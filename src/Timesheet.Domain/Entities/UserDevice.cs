using Timesheet.Shared.Abstractions.Entities;

namespace Timesheet.Domain.Entities;

public sealed class UserDevice : BaseEntity
{
    public Guid UserDeviceId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string? DeviceId { get; set; }
    public string? FcmToken { get; set; }
    public DateTime? FcmTokenExpiredAt { get; set; }
}