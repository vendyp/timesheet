using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Databases;
using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.Contracts.Requests;
using TimesheetService.WebApi.Contracts.Responses;
using TimesheetService.WebApi.Endpoints.RoleManagement.Scopes;
using TimesheetService.WebApi.Mapping;
using TimesheetService.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace TimesheetService.WebApi.Endpoints.RoleManagement;

public class GetRoleById : BaseEndpoint<GetRoleByIdRequest, RoleResponse>
{
    private readonly IDbContext _dbContext;

    public GetRoleById(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("roles/{RoleId}")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get roles by ID",
        Description = "",
        OperationId = "RoleManagement.GetRoleById",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(RoleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<RoleResponse>> HandleAsync([FromRoute] GetRoleByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new GetRoleByIdRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var role = await _dbContext.Set<Role>()
            .Include(e => e.RoleScopes)
            .Where(e => e.RoleId == request.RoleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
            return BadRequest(Error.Create("Data not found"));

        return role.ToRoleResponse();
    }
}