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
}