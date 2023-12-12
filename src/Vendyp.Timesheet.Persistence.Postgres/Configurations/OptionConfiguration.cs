using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendyp.Timesheet.Domain.Entities;
using Vendyp.Timesheet.Shared.Abstractions.Entities;

namespace Vendyp.Timesheet.Persistence.Postgres.Configurations;

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