using TimesheetService.WebApi.Common;
using TimesheetService.WebApi.Contracts.Responses;
using TimesheetService.WebApi.Endpoints.RoleManagement.Scopes;
using TimesheetService.WebApi.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace TimesheetService.WebApi.Endpoints.RoleManagement;

public class GetAllScope : BaseEndpoint<List<ScopeResponse>>
{
    [HttpGet("roles/scopes")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get all scopes API",
        Description = "",
        OperationId = "RoleManagement.GetAllScope",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(List<ScopeResponse>), StatusCodes.Status200OK)]
    public override Task<ActionResult<List<ScopeResponse>>> HandleAsync(
        CancellationToken cancellationToken = new())
    {
        var list = new List<ScopeResponse>();

        foreach (var item in ScopeManager.Instance.GetAllScopes())
            list.Add(new ScopeResponse
            {
                Name = item
            });

        return Task.FromResult<ActionResult<List<ScopeResponse>>>(list);
    }
}