using TimesheetService.Core.Abstractions;
using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Cache;
using TimesheetService.Shared.Abstractions.Clock;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Contracts.Responses;
using TimesheetService.WebApi.Endpoints.Identity.Helpers;
using TimesheetService.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TimesheetService.WebApi.Endpoints.Identity;

public class SignIn : BaseEndpoint<SignInRequest, LoginResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IClock _clock;
    private readonly IAuthManager _authManager;
    private readonly ICache _cache;

    public SignIn(IDbContext dbContext,
        IUserService userService,
        IClock clock,
        IAuthManager authManager,
        ICache cache)
    {
        _dbContext = dbContext;
        _userService = userService;
        _clock = clock;
        _authManager = authManager;
        _cache = cache;
    }

    [HttpPost("sign-in")]
    [SwaggerOperation(
        Summary = "Sign in",
        Description = "",
        OperationId = "Identity.SignIn",
        Tags = new[] { "Identity" })
    ]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(SignInRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new SignInRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var user = await _userService.GetByUsernameAsync(request.Username!, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create("Invalid username or password"));

        if (!_userService.VerifyPassword(user.Password!, user.Salt!, request.Password!))
            return BadRequest(Error.Create("Invalid username or password"));

        _dbContext.AttachEntity(user);

        var tsExpiry = new TimeSpan(0, 7, 0, 0);

        var newUserToken = new UserToken
        {
            UserId = user.UserId,
            RefreshToken = Guid.NewGuid().ToString("N"),
            ExpiryAt = _clock.CurrentDate().Add(tsExpiry),
        };
        user.UserTokens.Add(newUserToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var claims = ClaimsGenerator.Generate(user);

        var token = _authManager.CreateToken(newUserToken.UserTokenId.ToString("N"), _authManager.Options.Audience);

        var dto = new LoginResponse(user)
        {
            AccessToken = token.AccessToken,
            Expiry = token.Expiry,
            RefreshToken = newUserToken.RefreshToken
        };

        await _cache.SetAsync(newUserToken.UserTokenId.ToString("N"), claims, tsExpiry);

        return Ok(dto);
    }
}