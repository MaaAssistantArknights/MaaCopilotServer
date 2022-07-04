// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Services;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Infrastructure.Test.Services;

/// <summary>
/// Tests of <see cref="SecretService"/>.
/// </summary>
[TestClass]
public class SecretServiceTest
{
    /// <summary>
    /// The JWT option.
    /// </summary>
    private readonly IOptions<JwtOption> _jwtOption = Options.Create(new JwtOption()
    {
        Secret = "00000000000000000000000000000000",
        Issuer = "MaaCopilot",
        Audience = "Doctor",
        ExpireTime = 720,
    });

    /// <summary>
    /// Tests <see cref="SecretService.GenerateJwtToken(Guid)"/>.
    /// </summary>
    [TestMethod]
    public void TestGenerateJwtToken()
    {
        var service = new SecretService(_jwtOption);
        var (token, _) = service.GenerateJwtToken(Guid.Empty);
        token.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests <see cref="SecretService.GenerateToken(Guid, TimeSpan)"/>.
    /// </summary>
    [TestMethod]
    public void TestGenerateToken()
    {
        var service = new SecretService(_jwtOption);
        var (token, _) = service.GenerateToken(Guid.Empty, new TimeSpan(24, 0, 0));
        token.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests <see cref="SecretService.HashPassword(string)"/> and <see cref="SecretService.VerifyPassword(string, string)"/>.
    /// </summary>
    [TestMethod]
    public void TestHashPasswordAndVerifyPassword()
    {
        var password = "password";
        var service = new SecretService(_jwtOption);
        service.VerifyPassword(service.HashPassword(password), password).Should().BeTrue();
    }
}
