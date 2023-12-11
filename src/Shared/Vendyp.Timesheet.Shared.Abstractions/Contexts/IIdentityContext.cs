namespace Vendyp.Timesheet.Shared.Abstractions.Contexts;

public interface IIdentityContext
{
    bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Username { get; }
    Dictionary<string, IEnumerable<string>> Claims { get; }
    List<string> Roles { get; }
}