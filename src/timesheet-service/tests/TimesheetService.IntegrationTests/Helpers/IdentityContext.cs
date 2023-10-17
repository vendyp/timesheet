using TimesheetService.Shared.Abstractions.Contexts;

namespace TimesheetService.IntegrationTests.Helpers;

public class IdentityContext : IIdentityContext
{
    public IdentityContext(Guid userId)
    {
        IsAuthenticated = true;
        Id = userId;
        Username = Guid.NewGuid().ToString();
        Claims = new Dictionary<string, IEnumerable<string>>();
        Roles = new List<string>();
    }
    
    public bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Username { get; }
    public Dictionary<string, IEnumerable<string>> Claims { get; }
    public List<string> Roles { get; }
}