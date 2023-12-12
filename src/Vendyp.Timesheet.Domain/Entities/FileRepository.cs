using Vendyp.Timesheet.Domain.Enums;
using Vendyp.Timesheet.Shared.Abstractions.Entities;

namespace Vendyp.Timesheet.Domain.Entities;

public sealed class FileRepository : BaseEntity
{
    public Guid FileRepositoryId { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = null!;
    public string UniqueFileName { get; set; } = null!;
    public string FileExtension { get; set; } = null!;
    public long Size { get; set; }

    /// <summary>
    /// Source meaning identifier for business process use case
    /// </summary>
    public string? Source { get; set; }

    public string? Note { get; set; }

    /// <summary>
    /// Default value is <see cref="Enums.FileType.Others">FileType.Others</see>
    /// </summary>
    public FileType FileType { get; set; }

    /// <summary>
    /// Default value is <see cref="Enums.FileStoreAt.FileSystem">FileStoreAt.FileSystem</see>
    /// </summary>
    public FileStoreAt FileStoreAt { get; set; }

    /// <summary>
    /// Default value is false.
    /// </summary>
    public bool IsFileDeleted { get; set; }

    public DateTime? FileDeletedAt { get; set; }

    public void DeleteTheFile()
    {
        IsFileDeleted = true;
    }
}