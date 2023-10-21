using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Entities;

namespace TimesheetService.Persistence.SqlServer.Configurations;

public class TimesheetConfiguration : BaseEntityConfiguration<Timesheet>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Timesheet> builder)
    {
        builder.HasKey(e => e.TimesheetId);
        builder.Property(e => e.TimesheetId).ValueGeneratedNever();
        builder.Property(e => e.Title).HasMaxLength(256);

        //default description is text
        //builder.Property(e => e.Description)

        builder.Property(e => e.TotalTime)
            .HasPrecision(18, 2);
    }
}