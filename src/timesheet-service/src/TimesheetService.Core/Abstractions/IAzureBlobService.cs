using TimesheetService.Core.Models;

namespace TimesheetService.Core.Abstractions;

public interface IAzureBlobService
{
    Task<AzureBlobUploadResponse> UploadAsync(Stream stream, string containerName, string fileName,
        CancellationToken cancellationToken);

    Task<AzureBlobUriResponse> GenerateUriAsync(string containerName, string filename,
        CancellationToken cancellationToken);
}