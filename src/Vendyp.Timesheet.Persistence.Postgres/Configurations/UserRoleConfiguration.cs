using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vendyp.Timesheet.Domain.Entities;
using Vendyp.Timesheet.Shared.Abstractions.Entities;

namespace Vendyp.Timesheet.Persistence.Postgres.Configurations;

public class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });
        builder.Property(e => e.RoleId)
            .HasMaxLength(100);
    }
}