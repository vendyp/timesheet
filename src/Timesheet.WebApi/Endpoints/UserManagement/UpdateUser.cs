using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Databases;
using Timesheet.WebApi.Common;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Endpoints.UserManagement.Scopes;
using Timesheet.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Timesheet.WebApi.Endpoints.UserManagement;

public class UpdateUser : BaseEndpointWithoutResponse<UpdateUserRequest>
{
    private readonly IDbContext _dbContext;

    public UpdateUser(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPut("users/{userId}")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope))]
    [SwaggerOperation(
        Summary = "Update user API",
        Description = "",
        OperationId = "UserManagement.UpdateUser",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateUserRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new UpdateUserRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.UpdateUserRequestPayload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var user = await _dbContext.Set<User>().Where(e => e.UserId == request.UserId)
            .Select(e => new User
            {
                UserId = e.UserId,
                FullName = e.FullName
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            return BadRequest(Error.Create("Data not found"));

        _dbContext.AttachEntity(user);

        if (request.UpdateUserRequestPayload.Fullname != user.FullName)
            user.FullName = request.UpdateUserRequestPayload.Fullname;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}