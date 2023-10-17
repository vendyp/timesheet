namespace TimesheetService.WebApi.Contracts.Requests;

public class EditRoleRequestPayload
{
    public string? Description { get; set; }
    public List<string> Scopes { get; set; } = new();
}