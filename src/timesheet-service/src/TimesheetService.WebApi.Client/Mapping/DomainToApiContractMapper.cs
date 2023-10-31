using TimesheetService.Domain.Entities;
using TimesheetService.WebApi.Client.Contracts.Responses;

namespace TimesheetService.WebApi.Client.Mapping;

public static class DomainToApiContractMapper
{
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