using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Databases;
using Timesheet.WebApi.Common;
using Timesheet.WebApi.Contracts.Requests;
using Timesheet.WebApi.Endpoints.RoleManagement.Scopes;
using Timesheet.WebApi.Mapping;
using Timesheet.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Timesheet.WebApi.Endpoints.RoleManagement;

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