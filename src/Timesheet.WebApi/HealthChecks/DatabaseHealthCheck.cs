using System.Diagnostics;
using Timesheet.Domain.Entities;
using Timesheet.Shared.Abstractions.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Timesheet.WebApi.HealthChecks;

public class DatabaseHealthCheck : IHealthCheck
{
    private readonly IDbContext _dbContext;

    public DatabaseHealthCheck(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var sw = Stopwatch.StartNew();
            _ = await _dbContext.Set<User>().AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            sw.Stop();

            var s = sw.Elapsed.TotalMilliseconds;

            return s switch
            {
                < 500 => HealthCheckResult.Healthy("Database healthy"),
                > 500 => HealthCheckResult.Degraded("Database degraded"),
                _ => HealthCheckResult.Unhealthy("Database unhealthy")
            };
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message);
        }
    }
}