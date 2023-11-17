using Timesheet.Shared.Abstractions.Contexts;
using Microsoft.AspNetCore.Http;

namespace Timesheet.Shared.Infrastructure.Contexts;

internal sealed class Context : IContext
{
    public static string GetUserIpAddress(HttpContext? context)
    {
        if (context is null)
        {
            return string.Empty;
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        if (!context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
            return ipAddress ?? string.Empty;

        var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
        if (ipAddresses.Any())
            ipAddress = ipAddresses[0];

        return ipAddress ?? string.Empty;
    }

    public Guid RequestId { get; } = Guid.NewGuid();
    public string TraceId { get; }
    public string? IpAddress { get; }
    public string? UserAgent { get; }
    public IIdentityContext Identity { get; }

    public Context() : this($"{Guid.NewGuid():N}")
    {
    }

    public Context(HttpContext context) : this(context.TraceIdentifier,
        new IdentityContext(context.User), GetUserIpAddress(context),
        context.Request.Headers["user-agent"])
    {
    }

    public Context(string traceId, IIdentityContext? identity = null, string? ipAddress = null,
        string? userAgent = null)
    {
        TraceId = traceId;
        Identity = identity ?? IdentityContext.Empty;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    public static IContext Empty => new Context();
}