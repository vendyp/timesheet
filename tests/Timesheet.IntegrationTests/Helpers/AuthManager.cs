using Timesheet.WebApi.Common;

namespace Timesheet.IntegrationTests.Helpers;

public class AuthManager : IAuthManager
{
    public AuthManager()
    {
        Options = new AuthOptions();
    }

    public AuthOptions Options { get; }

    public JsonWebToken CreateToken(string uniqueIdentifier, string? audience = null,
        IDictionary<string, IEnumerable<string>>? claims = null) => new() { AccessToken = "abcde", Expiry = 1 };
}