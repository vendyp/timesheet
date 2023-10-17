using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using TimesheetService.Core.Abstractions;
using TimesheetService.Core.Models;
using Microsoft.Extensions.Configuration;

namespace TimesheetService.Infrastructure.Services;

public class AzureBlobService : IAzureBlobService
{
    private readonly BlobServiceClient _client;

    public AzureBlobService(IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("azureBlobStorage");
        if (string.IsNullOrWhiteSpace(connString))
            throw new InvalidOperationException("Connection string azure blob storage is null");
        _client = new BlobServiceClient(connString);
    }

    public async Task<AzureBlobUploadResponse> UploadAsync(Stream stream, string containerName, string fileName,
        CancellationToken cancellationToken)
    {
        await CreateContainerIfNotExistsAsync(containerName, cancellationToken);

        var containerClient = _client.GetBlobContainerClient(containerName);

        var name = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var blobClient = containerClient.GetBlobClient(name);

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = Path.GetExtension(fileName)
        };

        var blobUploadOptions = new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        };

        await blobClient.UploadAsync(stream, blobUploadOptions, cancellationToken);

        return new AzureBlobUploadResponse
        {
            NewFileName = blobClient.Name
        };
    }

    private async Task CreateContainerIfNotExistsAsync(string containerName, CancellationToken cancellationToken)
    {
        var blobContainer = _client.GetBlobContainerClient(containerName);
        await blobContainer.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<AzureBlobUriResponse> GenerateUriAsync(string containerName, string filename,
        CancellationToken cancellationToken)
    {
        await CreateContainerIfNotExistsAsync(containerName, cancellationToken);

        var containerClient = _client.GetBlobContainerClient(containerName);

        var blobClient = containerClient.GetBlobClient(filename);

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = filename,
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        // Generate the SAS token
        var sasToken = blobClient.GenerateSasUri(sasBuilder).ToString();

        return new AzureBlobUriResponse
        {
            Uri = sasToken
        };
    }
}