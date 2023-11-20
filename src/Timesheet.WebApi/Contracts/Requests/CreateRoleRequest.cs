namespace Timesheet.WebApi.Contracts.Requests;

public class CreateRoleRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string>? Scopes { get; set; } = new();
}