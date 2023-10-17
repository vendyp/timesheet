using TimesheetService.WebApi.Scopes;

namespace TimesheetService.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(UserManagementScope)}.readonly".ToLower();
}