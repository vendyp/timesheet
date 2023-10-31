namespace TimesheetService.WebApi.Client.Common;

public class JsonWebToken
{
    public string AccessToken { get; set; } = null!;
    public long Expiry { get; set; }
}