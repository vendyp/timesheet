using System.Security.Cryptography;
using TimesheetService.Shared.Abstractions.Encryption;

namespace TimesheetService.Shared.Infrastructure.Encryption;

internal sealed class Rng : IRng 
{
    private static readonly string[] SpecialChars = {"/", "\\", "=", "+", "?", ":", "&"};

    public string Generate(int length = 50, bool removeSpecialChars = true)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        var result = Convert.ToBase64String(bytes);

        return removeSpecialChars
            ? SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, string.Empty))
            : result;
    }
}