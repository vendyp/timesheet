using Timesheet.Shared.Abstractions.Queries;
using Timesheet.WebApi.Common;
using Timesheet.WebApi.Contracts.Responses;
using Timesheet.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Timesheet.WebApi.Endpoints.UserManagement;

public class GetAllUserOption : BaseEndpoint<QueryOption>
{
    [HttpOptions("users")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get user paginated options",
        Description = "",
        OperationId = "UserManagement.GetAllUserOption",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(QueryOption), StatusCodes.Status200OK)]
    public override Task<ActionResult<QueryOption>> HandleAsync(CancellationToken cancellationToken = new())
    {
        var options = new QueryOption();
        
        //setup columns
        options.Columns.Add(new Column(nameof(UserResponse.Username)) { EnableOrder = true });
        options.Columns.Add(new Column(nameof(UserResponse.FullName)));
        options.Columns.Add(new Column(nameof(UserResponse.Email)));
        options.Columns.Add(new Column(nameof(UserResponse.CreatedAt)) { EnableOrder = true });
        
        //filters
        options.Filters.Add(new Filter(nameof(UserResponse.Username)));
        options.Filters.Add(new Filter(nameof(UserResponse.FullName)));
        
        options.EnableGlobalSearch = false;
        
        return Task.FromResult<ActionResult<QueryOption>>(options);
    }
}