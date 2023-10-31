namespace TimesheetService.WebApi.Client.Contracts.Responses;

public class UserResponse : BaseResponse
{
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public DateTime? LastPasswordChangeAt { get; set; }
    public string? Email { get; set; }
    public List<string> Scopes { get; set; } = new();
}