using Vendyp.Timesheet.Core.Models;

namespace Vendyp.Timesheet.Core.Abstractions;

/// <summary>
/// Represents an interface for interacting with Azure Blob Storage.
/// </summary>
public interface IAzureBlobService
{
    /// <summary>
    /// Uploads a stream of data to Azure Blob Storage asynchronously.
    /// </summary>
    /// <param name="stream">The stream containing the data to be uploaded.</param>
    /// <param name="containerName">The name of the container where the file will be uploaded.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation. The task will complete with an AzureBlobUploadResponse object.</returns>
    Task<AzureBlobUploadResponse> UploadAsync(Stream stream, string containerName, string fileName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates a unique URI for the specified blob file in the given container.
    /// </summary>
    /// <param name="containerName">The name of the container.</param>
    /// <param name="filename">The name of the blob file.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated URI for the blob.</returns>
    Task<AzureBlobUriResponse> GenerateUriAsync(string containerName, string filename,
        CancellationToken cancellationToken = default);
}