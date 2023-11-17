using Timesheet.Domain.Entities;
using Timesheet.WebApi.Contracts.Responses;

namespace Timesheet.WebApi.Mapping;

public static class DomainToApiContractMapper
{
    public static RoleResponse ToRoleResponse(this Role role)
    {
        var response = new RoleResponse
        {
            RoleId = role.RoleId,
            Name = role.Name,
            Description = role.Description
        };

        foreach (var roleScope in role.RoleScopes)
            response.Scopes.Add(roleScope.ToScopeResponse());

        return response;
    }

    public static ScopeResponse ToScopeResponse(this RoleScope roleScope)
    {
        return new ScopeResponse
        {
            ScopeId = roleScope.RoleScopeId,
            Name = roleScope.Name
        };
    }

    public static UserResponse ToUserResponse(this User user)
    {
        var response = new UserResponse
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            LastPasswordChangeAt = user.LastPasswordChangeAt,
            Email = user.Email,
        };
        response.Scopes.AddRange(user.UserRoles.Select(e => e.Role).SelectMany(e => e!.RoleScopes).Select(e => e.Name));
        return response;
    }
}