using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendyp.Timesheet.Domain.Entities;
using Vendyp.Timesheet.Shared.Abstractions.Entities;

namespace Vendyp.Timesheet.Persistence.Postgres.Configurations;

public class FileRepositoryConfiguration : BaseEntityConfiguration<FileRepository>
{
    protected override void EntityConfiguration(EntityTypeBuilder<FileRepository> builder)
    {
        builder.HasKey(e => e.FileRepositoryId);
        builder.Property(e => e.FileRepositoryId).ValueGeneratedNever();

        builder.Property(e => e.FileName).HasMaxLength(256);

        builder.Property(e => e.UniqueFileName).HasMaxLength(256);

        builder.Property(e => e.FileExtension).HasMaxLength(256);

        builder.Property(e => e.Source).HasMaxLength(256);

        builder.Property(e => e.Note).HasMaxLength(512);

        builder.HasIndex(e => e.UniqueFileName).IsUnique();
    }
}