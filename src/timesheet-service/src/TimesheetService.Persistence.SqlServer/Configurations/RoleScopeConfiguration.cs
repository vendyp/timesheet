using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimesheetService.Persistence.SqlServer.Configurations;

public class RoleScopeConfiguration : BaseEntityConfiguration<RoleScope>
{
    protected override void EntityConfiguration(EntityTypeBuilder<RoleScope> builder)
    {
        builder.HasKey(e => e.RoleScopeId);
        builder.Property(e => e.RoleScopeId).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(256);
        builder.Property(e => e.Description).HasMaxLength(2048);
        builder.Property(e => e.RevokedMessage).HasMaxLength(2048);
    }
}