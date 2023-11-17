namespace Timesheet.Shared.Abstractions.Files;

public class FileRequest
{
    public FileRequest(string fileName, Stream stream)
    {
        FileName = fileName;
        Stream = stream;
    }

    public string FileName { get; }
    public Stream Stream { get; }
}