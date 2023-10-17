using TimesheetService.Shared.Abstractions.Clock;
using TimesheetService.Shared.Abstractions.Files;

namespace TimesheetService.Shared.Infrastructure.Files.FileSystems;

public class FileSystemService : IFileService
{
    private readonly FileSystemOptions _options;
    private readonly IClock _clock;

    public FileSystemService(FileSystemOptions options, IClock clock)
    {
        _options = options;
        _clock = clock;
    }

    public async Task<FileResponse> UploadAsync(FileRequest request, CancellationToken cancellationToken)
    {
        var fullPath = string.Empty;

        if (!string.IsNullOrWhiteSpace(_options.Path))
            fullPath = _options.Path;

        var now = _clock.CurrentDate();

        var newFileName =
            $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}_{Guid.NewGuid()}{Path.GetExtension(request.FileName)}";

        fullPath = Path.Combine(fullPath,
            now.Year.ToString().PadLeft(2, '0'),
            now.Month.ToString().PadLeft(2, '0'),
            now.Day.ToString().PadLeft(2, '0'));

        if (!Directory.Exists(fullPath))
            Directory.CreateDirectory(fullPath);

        fullPath = Path.Combine(fullPath, newFileName);

        await using (var fileStream = File.Create(fullPath))
        {
            await fileStream.CopyToAsync(fileStream, cancellationToken);
        }

        var response = new FileResponse(newFileName)
        {
            Path = fullPath
        };

        return response;
    }

    public Task<FileDownloadResponse?> DownloadAsync(string fileName, CancellationToken cancellationToken)
    {
        //20230103_test.txt
        var s = fileName.Split('_');
        if (s.Length != 2)
            throw new InvalidOperationException("Filename must contain _");

        if (s[0].Length != 8)
            throw new InvalidOperationException("Invalid format filename");

        //2023
        var year = s[0][..4];
        //01
        var month = s[0].Substring(4, 2);
        //03
        var day = s[0].Substring(6, 2);

        var fullPath = string.Empty;

        if (!string.IsNullOrWhiteSpace(_options.Path))
            fullPath = _options.Path;

        fullPath = Path.Combine(fullPath,
            year,
            month,
            day,
            fileName);

        return Task.FromResult(!File.Exists(fullPath)
            ? null
            : new FileDownloadResponse(fileName, File.OpenRead(fullPath)));
    }
}