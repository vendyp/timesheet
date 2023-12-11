namespace Vendyp.Timesheet.Shared.Abstractions.Files;

public class FileDownloadResponse
{
    public FileDownloadResponse(string fileName, Stream stream)
    {
        FileName = fileName;
        Stream = stream;
    }

    public string FileName { get; }
    public Stream Stream { get; }
}