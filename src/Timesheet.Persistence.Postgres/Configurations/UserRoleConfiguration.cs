using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timesheet.Persistence.Postgres.Configurations;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });
        builder.Property(e => e.RoleId)
            .HasMaxLength(100);
    }
}