using Timesheet.WebApi.Scopes;

namespace Timesheet.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(UserManagementScope)}.readonly".ToLower();
}