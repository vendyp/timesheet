using Microsoft.EntityFrameworkCore;
using TimesheetService.Core.Abstractions;
using TimesheetService.Domain.Entities;
using TimesheetService.Shared.Abstractions.Databases;

namespace TimesheetService.Infrastructure.Services;

public class TimesheetService : ITimesheetService
{
    private readonly IDbContext _dbContext;

    public TimesheetService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Timesheet> GetBaseQuery()
        => _dbContext.Set<Timesheet>()
            .AsQueryable();

    public Task<Timesheet?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => _dbContext.Set<Timesheet>()
            .Where(e => e.TimesheetId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<Timesheet>> GetAllAsync(CancellationToken cancellationToken)
        => _dbContext.Set<Timesheet>()
            .ToListAsync(cancellationToken);

    public async Task<Timesheet?> CreateAsync(Timesheet entity, CancellationToken cancellationToken)
    {
        _dbContext.Insert(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var timesheet = await GetBaseQuery()
            .Select(e => new Timesheet
            {
                TimesheetId = e.TimesheetId
            })
            .Where(e => e.TimesheetId == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (timesheet is null)
            throw new Exception("Data timesheet is null");

        _dbContext.AttachEntity(timesheet);

        timesheet.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<decimal> GetTotalHoursByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => GetBaseQuery()
            .Where(e => e.UserId == userId)
            .Select(e => e.TotalTime)
            .SumAsync(cancellationToken);
}