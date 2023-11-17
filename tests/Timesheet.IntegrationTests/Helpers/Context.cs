using Timesheet.Shared.Abstractions.Contexts;

namespace Timesheet.IntegrationTests.Helpers;

public class Context : IContext
{
    public Context(Guid id)
    {
        RequestId = Guid.NewGuid();
        TraceId = Guid.NewGuid().ToString();
        IpAddress = null;
        UserAgent = null;
        Identity = new IdentityContext(id);
    }
    
    public Guid RequestId { get; }
    public string TraceId { get; }
    public string? IpAddress { get; }
    public string? UserAgent { get; }
    public IIdentityContext Identity { get; }
}