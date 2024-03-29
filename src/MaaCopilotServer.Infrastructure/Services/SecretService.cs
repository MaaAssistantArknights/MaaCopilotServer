// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MaaCopilotServer.Infrastructure.Services;

/// <summary>
///     The service for processing passwords and tokens.
/// </summary>
public class SecretService : ISecretService
{
    private readonly IOptions<JwtOption> _jwtOption;


    /// <summary>
    ///     The constructor of <see cref="SecretService" />.
    /// </summary>
    /// <param name="jwtOption"></param>
    public SecretService(IOptions<JwtOption> jwtOption)
    {
        _jwtOption = jwtOption;
    }

    /// <inheritdoc />
    public (string, DateTimeOffset) GenerateJwtToken(Guid id)
    {
        var claims = new List<Claim> { new("id", id.ToString()) };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.Secret));

        var expire = DateTimeOffset.UtcNow.AddHours(_jwtOption.Value.ExpireTime);

        var token = new JwtSecurityToken(
            _jwtOption.Value.Issuer,
            _jwtOption.Value.Audience,
            claims,
            expires: expire.DateTime,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expire);
    }

    /// <inheritdoc />
    public (string, DateTimeOffset) GenerateRefreshToken()
    {
        var expires = DateTimeOffset.UtcNow.AddHours(_jwtOption.Value.RefreshExpireTime);
        
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return (Convert.ToBase64String(randomNumber), expires);
    }

    /// <inheritdoc />
    public (string, DateTimeOffset) GenerateToken(Guid resourceId, TimeSpan validTimeSpan)
    {
        var validBefore = DateTimeOffset.UtcNow.Add(validTimeSpan);

        var str = $"{resourceId}{validBefore.ToIsoString()}";
        var md5 = MD5.Create();
        var buff = Encoding.UTF8.GetBytes(str);
        var hashedBytes = md5.ComputeHash(buff);
        var hash = BitConverter.ToString(hashedBytes).Replace("-", string.Empty).ToUpper().AsSpan();

        // ReSharper disable once ReplaceSliceWithRangeIndexer
#pragma warning disable IDE0057
        var sec1 = hash.Slice(0, 6);
#pragma warning restore IDE0057
        var sec2 = hash.Slice(6, 10);
        var sec3 = hash.Slice(16, 4);
        var sec4 = hash.Slice(20, 8);
        var sec5 = hash.Slice(28, 4);

        var code = $"{sec1}-{sec2}-{sec3}-{sec4}-{sec5}";

        return (code, validBefore);
    }

    /// <inheritdoc />
    public Guid? GetUserIdFromAccessToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOption.Value.Issuer,
            ValidAudience = _jwtOption.Value.Audience,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.Value.Secret))
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken token ||
            token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase) is false)
        {
            return null;
        }

        var id = principal.FindFirst("id")?.Value;
        var canParse = Guid.TryParse(id, out var guid);

        return canParse ? guid : null;
    }

    /// <inheritdoc />
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <inheritdoc />
    public bool VerifyPassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
