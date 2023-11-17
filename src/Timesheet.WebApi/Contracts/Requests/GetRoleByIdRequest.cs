using Microsoft.AspNetCore.Mvc;

namespace Timesheet.WebApi.Contracts.Requests;

public class GetRoleByIdRequest
{
    [FromRoute] public Guid RoleId { get; set; }
}