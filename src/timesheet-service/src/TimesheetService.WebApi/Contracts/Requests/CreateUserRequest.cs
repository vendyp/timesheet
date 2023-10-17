namespace TimesheetService.WebApi.Contracts.Requests;

public class CreateUserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Fullname { get; set; }
    public Guid? RoleId { get; set; }
    public string? EmailAddress { get; set; }
}