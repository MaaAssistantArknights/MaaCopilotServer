// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MaaCopilotServer.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MaaCopilotServer.Infrastructure.Services;

/// <summary>
/// The service for processing passwords and tokens.
/// </summary>
public class SecretService : ISecretService
{
    /// <summary>
    /// The configuration.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// The constructor of <see cref="SecretService"/>.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public SecretService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JWT token.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>A JWT token string, and the expiration time.</returns>
    public (string, DateTimeOffset) GenerateJwtToken(Guid id)
    {
        var claims = new List<Claim> { new("id", id.ToString()) };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

        var expire = DateTimeOffset.UtcNow.AddHours(_configuration.GetValue<int>("Jwt:ExpireTime"));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expire.DateTime,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expire);
    }

    /// <summary>
    /// Hashes a password with the salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password.</returns>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a password to check if it matches the hash.
    /// </summary>
    /// <param name="hash">The hash of the password.</param>
    /// <param name="password">The password.</param>
    /// <returns><c>true</c> if the password matches the hash, <c>false</c> otherwise.</returns>
    public bool VerifyPassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
