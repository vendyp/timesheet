using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timesheet.Persistence.Postgres.Configurations;

public class OptionConfiguration : BaseEntityConfiguration<Option>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Option> builder)
    {
        builder.HasKey(e => e.Key);
        builder.Property(e => e.Key).HasMaxLength(256)
            .ValueGeneratedNever();
        builder.Property(e => e.Value).HasMaxLength(512);
    }
}