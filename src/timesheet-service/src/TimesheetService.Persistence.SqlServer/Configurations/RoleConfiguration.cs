using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimesheetService.Persistence.SqlServer.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.RoleId);
        builder.Property(e => e.RoleId).ValueGeneratedNever();
        builder.Property(e => e.Code).HasMaxLength(256);
        builder.HasIndex(e => e.Code);
        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.Description).HasMaxLength(512);
    }
}