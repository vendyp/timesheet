using Microsoft.AspNetCore.Mvc;

namespace Timesheet.WebApi.Contracts.Requests;

public class EditRoleRequest
{
    [FromRoute] public Guid RoleId { get; set; }
    [FromBody] public EditRoleRequestPayload Payload { get; set; } = null!;
}