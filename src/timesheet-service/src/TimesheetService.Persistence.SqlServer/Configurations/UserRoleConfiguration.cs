using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimesheetService.Persistence.SqlServer.Configurations;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });
        builder.Property(e => e.RoleId)
            .HasMaxLength(100);
    }
}