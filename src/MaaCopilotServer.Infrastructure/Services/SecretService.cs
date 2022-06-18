// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MaaCopilotServer.Infrastructure.Services;

/// <summary>
/// The service for processing passwords and tokens.
/// </summary>
public class SecretService : ISecretService
{
    /// <summary>
    /// Jwt options.
    /// </summary>
    private readonly IOptions<JwtOption> _jwtOption;


    /// <summary>
    /// The constructor of <see cref="SecretService"/>.
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
    public (string, DateTimeOffset) GenerateToken(Guid resourceId, TimeSpan validTimeSpan)
    {
        var expireTime = DateTimeOffset.UtcNow.AddMinutes(validTimeSpan.TotalMinutes);
        var eb = (ReadOnlySpan<byte>)BitConverter.GetBytes(expireTime.Ticks).AsSpan();
        var rb = (ReadOnlySpan<byte>)resourceId.ToByteArray().AsSpan();
        var span = new Span<byte>(new byte[eb.Length + rb.Length]);
        eb.CopyTo(span);
        rb.CopyTo(span[eb.Length..]);
        var outputSpan = new Span<byte>(new byte[16]);
        MD5.HashData(span, outputSpan);
        var token = Convert.ToBase64String(outputSpan);
        return (token, expireTime);
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
