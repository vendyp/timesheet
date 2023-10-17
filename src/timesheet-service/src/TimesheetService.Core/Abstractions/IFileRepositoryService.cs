﻿using TimesheetService.Domain.Entities;

namespace TimesheetService.Core.Abstractions;

public interface IFileRepositoryService : IEntityService<FileRepository>
{
    Task<long> TotalUsedStorageAsync(CancellationToken cancellationToken);
    Task<FileRepository?> GetByUniqueFileNameAsync(string fileName, CancellationToken cancellationToken);
}