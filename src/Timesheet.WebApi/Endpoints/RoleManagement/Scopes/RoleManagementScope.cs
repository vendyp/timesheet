using Timesheet.WebApi.Scopes;

namespace Timesheet.WebApi.Endpoints.RoleManagement.Scopes;

public class RoleManagementScope : IScope
{
    public string ScopeName => nameof(RoleManagementScope).ToLower();
}