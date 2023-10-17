using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Endpoints.RoleManagement.Scopes;
using TimesheetService.WebApi.Mapping;
using TimesheetService.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace TimesheetService.WebApi.Endpoints.RoleManagement;

public class CreateRole : BaseEndpointWithoutResponse<CreateRoleRequest>
{
    private readonly IDbContext _dbContext;

    public CreateRole(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("roles")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope))]
    [SwaggerOperation(
        Summary = "Create role API",
        Description = "",
        OperationId = "RoleManagement.CreateRole",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateRoleRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateRoleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var roleIsExists = await _dbContext.Set<Role>().Where(e => e.Name == request.Name)
            .FirstOrDefaultAsync(cancellationToken);
        if (roleIsExists != null)
            return BadRequest(Error.Create($"Role name {request.Name} already exists"));

        var role = request.ToRole();

        _dbContext.Insert(role);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}