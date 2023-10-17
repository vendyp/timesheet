namespace TimesheetService.WebApi.Contracts.Requests;

public class UploadFileRequest
{
    public IFormFile File { get; set; } = null!;
    public string Source { get; set; } = null!;
}