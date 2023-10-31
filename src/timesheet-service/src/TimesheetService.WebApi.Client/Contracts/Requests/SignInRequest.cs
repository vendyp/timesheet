namespace TimesheetService.WebApi.Client.Contracts.Requests;

public class SignInRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}