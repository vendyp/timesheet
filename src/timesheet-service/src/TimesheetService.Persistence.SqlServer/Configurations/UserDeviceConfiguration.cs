using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TimesheetService.Persistence.SqlServer.Configurations;

public class UserDeviceConfiguration : BaseEntityConfiguration<UserDevice>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserDevice> builder)
    {
        builder.HasKey(e => e.UserDeviceId);
        builder.Property(e => e.UserDeviceId).ValueGeneratedNever();
        builder.Property(e => e.DeviceId).HasMaxLength(256);
        builder.Property(e => e.FcmToken).HasMaxLength(1024);
    }
}