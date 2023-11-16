using TimesheetService.Domain.Enums;
using TimesheetService.Shared.Abstractions.Entities;

namespace TimesheetService.Domain.Entities;

public class Timesheet : BaseEntity
{
    public Guid TimesheetId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal TotalTime { get; set; }
    public TimesheetStatus Status { get; set; } = TimesheetStatus.Requested;
    public DateTime? ApprovedAt { get; set; }
    public DateTime? ApprovedAtServer { get; set; }
    public string? RejectedMessage { get; set; }

    public void Rejected(string message, DateTime? approvedAt, DateTime? approvedAtServer)
    {
        RejectedMessage = message;
        ApprovedAt = approvedAt;
        ApprovedAtServer = approvedAtServer;
    }
}