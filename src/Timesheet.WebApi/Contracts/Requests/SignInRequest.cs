namespace Timesheet.WebApi.Contracts.Requests;

public class SignInRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}