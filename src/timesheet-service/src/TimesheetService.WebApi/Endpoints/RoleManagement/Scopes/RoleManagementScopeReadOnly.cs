using TimesheetService.WebApi.Scopes;

namespace TimesheetService.WebApi.Endpoints.RoleManagement.Scopes;

public class RoleManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(RoleManagementScope)}.readonly".ToLower();
}