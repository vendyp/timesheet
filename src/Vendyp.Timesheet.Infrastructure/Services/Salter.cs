using Vendyp.Timesheet.Shared.Abstractions.Encryption;

namespace Vendyp.Timesheet.Infrastructure.Services;

public class Salter : ISalter
{
    private readonly ISha512 _sha512;

    public Salter(ISha512 sha512)
    {
        _sha512 = sha512;
    }

    public string Hash(string salt, string password)
        => _sha512.Hash($"{salt}_@_{password}");
}