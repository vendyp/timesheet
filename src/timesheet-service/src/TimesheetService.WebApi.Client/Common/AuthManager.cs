using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TimesheetService.Shared.Abstractions.Clock;

namespace TimesheetService.WebApi.Client.Common;

public class AuthManager : IAuthManager
{
    private readonly IClock _clock;
    private readonly SigningCredentials _signingCredentials;
    private readonly string? _issuer;

    public AuthManager(AuthOptions options, IClock clock)
    {
        var issuerSigningKey = options.IssuerSigningKey;
        if (issuerSigningKey is null)
        {
            throw new InvalidOperationException("Issuer signing key not set.");
        }

        Options = options;
        _clock = clock;
        _signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.IssuerSigningKey!)),
                SecurityAlgorithms.HmacSha256);
        _issuer = options.Issuer;
    }

    public AuthOptions Options { get; }

    public JsonWebToken CreateToken(string uniqueIdentifier, string? audience = null,
        IDictionary<string, IEnumerable<string>>? claims = null)
    {
        var now = _clock.CurrentDate();
        var jwtClaims = new List<Claim>
        {
            new("uir", uniqueIdentifier),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeMilliseconds().ToString())
        };

        if (!string.IsNullOrWhiteSpace(audience))
        {
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
        }

        if (claims?.Any() is true)
        {
            var customClaims = new List<Claim>();
            foreach (var (claim, values) in claims)
            {
                customClaims.AddRange(values.Select(value => new Claim(claim, value)));
            }

            jwtClaims.AddRange(customClaims);
        }

        var expires = now.Add(Options.Expiry);

        var jwt = new JwtSecurityToken(
            _issuer,
            claims: jwtClaims,
            notBefore: now,
            expires: expires,
            signingCredentials: _signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        var jsonWebToken = new JsonWebToken
        {
            AccessToken = token,
            Expiry = new DateTimeOffset(expires).ToUnixTimeMilliseconds(),
        };

        return jsonWebToken;
    }
}