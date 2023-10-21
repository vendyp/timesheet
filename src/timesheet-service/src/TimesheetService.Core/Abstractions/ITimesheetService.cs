using TimesheetService.Domain.Entities;

namespace TimesheetService.Core.Abstractions;

public interface ITimesheetService : IEntityService<Timesheet>
{
    Task<decimal> GetTotalHoursByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}