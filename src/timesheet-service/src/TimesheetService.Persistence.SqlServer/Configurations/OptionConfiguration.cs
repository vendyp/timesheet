using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimesheetService.Persistence.SqlServer.Configurations;

public class OptionConfiguration : BaseEntityConfiguration<Option>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Option> builder)
    {
        builder.HasKey(e => e.Key);
        builder.Property(e => e.Key).HasColumnType("varchar")
            .HasMaxLength(256)
            .ValueGeneratedNever();
        builder.Property(e => e.Value).HasMaxLength(512);
        builder.Property(e => e.Description).HasColumnType("text");
    }
}