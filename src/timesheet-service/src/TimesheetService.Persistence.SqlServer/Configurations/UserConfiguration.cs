using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimesheetService.Persistence.SqlServer.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    protected override void EntityConfiguration(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId).ValueGeneratedNever();
        builder.Property(e => e.Username).HasColumnType("varchar")
            .HasMaxLength(256);
        builder.Property(e => e.NormalizedUsername).HasColumnType("varchar")
            .HasMaxLength(256);
        builder.Property(e => e.Salt).HasColumnType("varchar")
            .HasMaxLength(512);
        builder.Property(e => e.Password).HasColumnType("varchar")
            .HasMaxLength(512);
        builder.Property(e => e.FullName).HasMaxLength(512);
        builder.Property(e => e.Email).HasColumnType("varchar")
            .HasMaxLength(256);
    }
}