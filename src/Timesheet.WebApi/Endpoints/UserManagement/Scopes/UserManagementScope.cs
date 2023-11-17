using Timesheet.WebApi.Scopes;

namespace Timesheet.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScope : IScope
{
    public string ScopeName => nameof(UserManagementScope).ToLower();
}