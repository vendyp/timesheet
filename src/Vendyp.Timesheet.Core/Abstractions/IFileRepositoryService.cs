using Vendyp.Timesheet.Domain.Entities;

namespace Vendyp.Timesheet.Core.Abstractions;

/// <summary>
/// Represents a service for interacting with a file repository.
/// </summary>
public interface IFileRepositoryService : IEntityService<FileRepository>
{
    /// <summary>
    /// Calculates the total used storage asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>The total used storage in bytes.</returns>
    Task<long> TotalUsedStorageAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a file from the repository based on its unique file name.
    /// </summary>
    /// <param name="fileName">The unique file name of the file to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="FileRepository"/> object representing the file if found; otherwise, <see langword="null"/>.</returns>
    Task<FileRepository?> GetByUniqueFileNameAsync(string fileName, CancellationToken cancellationToken = default);
}