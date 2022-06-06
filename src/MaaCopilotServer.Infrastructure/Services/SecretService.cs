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

public class SecretService : ISecretService
{
    private readonly IConfiguration _configuration;

    public SecretService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public (string, DateTimeOffset) GenerateJwtToken(Guid id)
    {
        var claims = new List<Claim> { new("id", id.ToString())};

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

    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
