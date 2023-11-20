using Microsoft.AspNetCore.Mvc;

namespace Timesheet.WebApi.Contracts.Requests;

public class EditRoleRequest
{
    [FromRoute] public Guid RoleId { get; set; }
    [FromBody] public EditRoleRequestPayload Payload { get; set; } = null!;
}

public class EditRoleRequestPayload
{
    public string? Description { get; set; }
    public List<string> Scopes { get; set; } = new();
}