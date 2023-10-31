using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TimesheetService.WebApi.Client.Scopes;

namespace TimesheetService.WebApi.Client.Common;

[AttributeUsage(AttributeTargets.Method)]
public class RequiredScopeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly bool _single = true;
    private readonly string _scopeName;

    public RequiredScopeAttribute(string scopeName)
    {
        _scopeName = scopeName;
    }

    /// <summary>
    /// Either one of those scope types will satisfied, default using IScope.ScopeName
    /// </summary>
    /// <param name="types">If only one, that must satisfied, if more than one, either one of them</param>
    public RequiredScopeAttribute(params Type[] types)
    {
        if (!types.Any())
            throw new InvalidOperationException("At least 1 passed");

        _scopeName = string.Empty;
        _single = types.Length == 1;

        var typeOfIScope = typeof(IScope);

        // validates types must assignable from IScope
        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];

            if (!typeOfIScope.IsAssignableFrom(type))
                throw new InvalidOperationException("Type passed through required scope must assignable by IScope");

            var instance = Activator.CreateInstance(type)!;
            var propertyInfoName = type.GetProperty(nameof(IScope.ScopeName))!;

            if (types.Length - 1 == i)
                _scopeName += (propertyInfoName.GetValue(instance)! as string)!;
            else
                _scopeName += $"{(propertyInfoName.GetValue(instance)! as string)!},";
        }
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            context.Result = new ForbidResult();
            return;
        }

        var claims = user.Claims
            .Where(c => c.Type == "scopes")
            .ToList();

        if (!claims.Any())
        {
            context.Result = new ForbidResult();
            return;
        }

        if (_single)
        {
            if (claims.Any(e => e.Value == _scopeName))
                return;
        }
        else
        {
            var scopes = _scopeName.Split(',');
            var passed = false;
            foreach (var item in scopes)
            {
                if (claims.Any(e => e.Value == item))
                    passed = true;
            }

            if (passed)
                return;
        }

        context.Result = new ForbidResult();
    }
}