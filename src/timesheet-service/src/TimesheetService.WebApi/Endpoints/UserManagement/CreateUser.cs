using TimesheetService.Core.Abstractions;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.Shared.Abstractions.Encryption;
using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Endpoints.UserManagement.Scopes;
using TimesheetService.WebApi.Mapping;
using TimesheetService.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TimesheetService.WebApi.Endpoints.UserManagement;

public class CreateUser : BaseEndpointWithoutResponse<CreateUserRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IRng _rng;
    private readonly ISalter _salter;

    public CreateUser(IDbContext dbContext, IUserService userService, IRng rng, ISalter salter)
    {
        _dbContext = dbContext;
        _userService = userService;
        _rng = rng;
        _salter = salter;
    }

    [HttpPost("users")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope))]
    [SwaggerOperation(
        Summary = "Create user API",
        Description = "",
        OperationId = "UserManagement.CreateUser",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateUserRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var userExist = await _userService.IsUserExistAsync(request.Username!, cancellationToken);
        if (userExist)
            return BadRequest(Error.Create("Username already exists"));

        var user = request.ToUser(
            _rng.Generate(128, false),
            _salter);

        _dbContext.Insert(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}