using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TimesheetService.WebApi.Client.HealthChecks;

public class ApplicationHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        return Task.FromResult(HealthCheckResult.Healthy("Application run perfectly.."));
    }
}