using Timesheet.Shared.Abstractions.Files;

namespace Timesheet.IntegrationTests.Helpers;

public class FileSystemServiceMock : IFileService
{
    public Task<FileResponse> UploadAsync(FileRequest request, CancellationToken cancellationToken)
        => Task.FromResult(new FileResponse($"{Guid.NewGuid()}{Path.GetExtension(request.FileName)}"));

    public Task<FileDownloadResponse?> DownloadAsync(string fileName, CancellationToken cancellationToken)
        => Task.FromResult<FileDownloadResponse?>(null);
}