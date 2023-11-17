using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Timesheet.Persistence.Postgres.Configurations;

public class UserTokenConfiguration : BaseEntityConfiguration<UserToken>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey(e => e.UserTokenId);
        builder.Property(e => e.UserTokenId).ValueGeneratedNever();
        builder.Property(e => e.RefreshToken)
            .HasMaxLength(256);
    }
}