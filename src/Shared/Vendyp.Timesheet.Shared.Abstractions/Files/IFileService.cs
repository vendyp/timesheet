﻿namespace Vendyp.Timesheet.Shared.Abstractions.Files;

public interface IFileService
{
    Task<FileResponse> UploadAsync(FileRequest request, CancellationToken cancellationToken);
    Task<FileDownloadResponse?> DownloadAsync(string fileName, CancellationToken cancellationToken);
}