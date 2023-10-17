using TimesheetService.Core.Abstractions;
using TimesheetService.Shared.Abstractions.Clock;
using TimesheetService.Shared.Abstractions.Contexts;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TimesheetService.WebApi.Endpoints.Identity;

public class ChangePassword : BaseEndpointWithoutResponse<ChangePasswordRequest>
{
    private readonly IUserService _userService;
    private readonly ISalter _salter;
    private readonly IClock _clock;
    private readonly IDbContext _dbContext;
    private readonly IRng _rng;
    private readonly IContext _context;

    public ChangePassword(IUserService userService,
        ISalter salter,
        IClock clock,
        IDbContext dbContext,
        IRng rng,
        IContext context)
    {
        _userService = userService;
        _salter = salter;
        _clock = clock;
        _dbContext = dbContext;
        _rng = rng;
        _context = context;
    }

    [HttpPut("password")]
    [SwaggerOperation(
        Summary = "Change Password",
        Description = "Change Password API dedicated for identity",
        OperationId = "Identity.ChangePassword",
        Tags = new[] { "Identity" })
    ]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var validator = new ChangePasswordRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var user = await _userService.GetByIdAsync(_context.Identity.Id, cancellationToken);
        if (user?.Password is null)
            return BadRequest(Error.Create("Data not found"));

        if (!_userService.VerifyPassword(user.Password!, user.Salt!, request.CurrentPassword!))
            return BadRequest(Error.Create("Invalid password"));

        _dbContext.AttachEntity(user);

        user.Salt = _rng.Generate(128);
        user.Password = _salter.Hash(user.Salt, request.NewPassword!);
        user.LastPasswordChangeAt = _clock.CurrentDate();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}