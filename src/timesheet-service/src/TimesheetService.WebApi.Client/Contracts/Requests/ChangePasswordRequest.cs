namespace TimesheetService.WebApi.Client.Contracts.Requests;

public class ChangePasswordRequest
{
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}