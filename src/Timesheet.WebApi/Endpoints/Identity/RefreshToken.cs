using Timesheet.Core.Abstractions;
using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Cache;
using Timesheet.Shared.Abstractions.Clock;
using Timesheet.Shared.Abstractions.Databases;
using Timesheet.WebApi.Common;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Contracts.Responses;
using Timesheet.WebApi.Endpoints.Identity.Helpers;
using Timesheet.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Timesheet.WebApi.Endpoints.Identity;

public class RefreshToken : BaseEndpoint<RefreshTokenRequest, LoginResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IClock _clock;
    private readonly IUserService _userService;
    private readonly IAuthManager _authManager;
    private readonly ICache _cache;

    public RefreshToken(IDbContext dbContext, IClock clock, IUserService userService, IAuthManager authManager,
        ICache cache)
    {
        _dbContext = dbContext;
        _clock = clock;
        _userService = userService;
        _authManager = authManager;
        _cache = cache;
    }

    [HttpPost("refresh")]
    [SwaggerOperation(
        Summary = "Refresh token",
        Description = "",
        OperationId = "Identity.Refresh",
        Tags = new[] { "Identity" })
    ]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(RefreshTokenRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new RefreshTokenRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var userToken = await _dbContext.Set<UserToken>()
            .Where(e => e.RefreshToken == request.RefreshToken)
            .Select(e => new UserToken
            {
                UserTokenId = e.UserTokenId,
                RefreshToken = e.RefreshToken,
                IsUsed = e.IsUsed,
                ExpiryAt = e.ExpiryAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userToken is null || userToken.IsUsed || userToken.ExpiryAt < _clock.CurrentDate())
            return BadRequest(Error.Create("Invalid request"));

        _dbContext.AttachEntity(userToken);

        userToken.IsUsed = true;

        await _cache.DeleteAsync(userToken.UserTokenId.ToString("N"));

        var tsExpiry = new TimeSpan(0, 7, 0, 0);

        var newUserToken = new UserToken
        {
            UserId = userToken.UserId,
            RefreshToken = Guid.NewGuid().ToString("N"),
            ExpiryAt = _clock.CurrentDate().Add(tsExpiry),
        };

        _dbContext.Set<UserToken>().Add(newUserToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _userService.GetByIdAsync(userToken.UserId, cancellationToken);

        var claims = ClaimsGenerator.Generate(user!);

        var token = _authManager.CreateToken(newUserToken.UserTokenId.ToString("N"), _authManager.Options.Audience);

        var dto = new LoginResponse(user!)
        {
            AccessToken = token.AccessToken,
            Expiry = token.Expiry,
            RefreshToken = newUserToken.RefreshToken
        };

        await _cache.SetAsync(newUserToken.UserTokenId.ToString("N"), claims, tsExpiry);

        return Ok(dto);
    }
}