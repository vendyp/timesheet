using Timesheet.WebApi.Scopes;

namespace Timesheet.WebApi.Endpoints.RoleManagement.Scopes;

public class RoleManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(RoleManagementScope)}.readonly".ToLower();
}