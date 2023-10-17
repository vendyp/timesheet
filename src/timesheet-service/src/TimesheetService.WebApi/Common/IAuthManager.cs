namespace TimesheetService.WebApi.Common;

public interface IAuthManager
{
    AuthOptions Options { get; }
    JsonWebToken CreateToken(string uniqueIdentifier, string? audience = null,
        IDictionary<string, IEnumerable<string>>? claims = null);
}