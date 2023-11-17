namespace Timesheet.Shared.Abstractions.Encryption;

public interface ISalter
{
    string Hash(string salt, string password);
}