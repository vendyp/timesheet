namespace Timesheet.WebApi.Contracts.Responses;

public record RoleResponse
{
    public Guid? RoleId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<ScopeResponse> Scopes { get; set; } = new();
}