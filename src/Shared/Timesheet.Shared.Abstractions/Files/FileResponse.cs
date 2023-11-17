namespace Timesheet.Shared.Abstractions.Files;

public class FileResponse
{
    public FileResponse(string newFileName)
    {
        NewFileName = newFileName;
    }

    public string NewFileName { get; set; }
    public string? Path { get; set; }
}