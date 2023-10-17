using Microsoft.AspNetCore.Mvc;

namespace TimesheetService.WebApi.Contracts.Requests;

public class GetRoleByIdRequest
{
    [FromRoute] public Guid RoleId { get; set; }
}