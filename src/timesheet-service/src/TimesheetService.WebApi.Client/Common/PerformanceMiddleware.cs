using System.Diagnostics;

namespace TimesheetService.WebApi.Client.Common;

internal class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMiddleware> _logger;

    public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var sw = Stopwatch.StartNew();
        await _next(httpContext);
        sw.Stop();

        if (sw.Elapsed.TotalSeconds > 1)
        {
            _logger.LogWarning("API Performance degraded, it took {TotalMillis}ms to get response from: {Path}",
                sw.Elapsed.TotalMilliseconds, httpContext.Request.Path);
        }
    }
}