using System.Security.Claims;
using TimesheetService.Domain.Entities;
using TimesheetService.Domain.Extensions;
using TimesheetService.WebApi.Scopes;

namespace TimesheetService.WebApi.Endpoints.Identity.Helpers;

public static class ClaimsGenerator
{
    public static Dictionary<string, IEnumerable<string>> Generate(User user)
    {
        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["xid"] = new[] { user.UserId.ToString() },
            ["usr"] = new[] { user.Username },
            [ClaimTypes.Name] = new[] { user.UserId.ToString() }
        };

        foreach (var userRole in user.UserRoles)
        {
            claims.Add(ClaimTypes.Role, new[] { userRole.RoleId.ToString() });

            if (userRole.Role != null && userRole.Role.RoleScopes.Any())
            {
                claims.Add("scopes", userRole.Role!.RoleScopes.Select(e => e.Name));
            }
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(ClaimTypes.Email, new[] { user.Email });

        //if normal
        if (claims[ClaimTypes.Role].Any(e => e != RoleExtensions.SuperAdministratorId.ToString()))
            return claims;

        claims.Remove("scopes");

        claims.Add("scopes", ScopeManager.Instance.GetAllScopes());

        return claims;
    }
}