// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Test.TestHelpers;
using Moq;

namespace MaaCopilotServer.Application.Test.TestExtensions;

/// <summary>
/// Test extension for <see cref="ISecretService"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class SecretServiceTestExtension
{

    /// <summary>
    /// Setups <see cref="ISecretService.GenerateJwtToken(Guid)"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="userEntity">The test user entity.</param>
    /// <param name="returnedToken">The returned token.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupGenerateJwtToken(this Mock<ISecretService> mock, Guid userEntity, string returnedToken = HandlerTest.TestToken)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.GenerateJwtToken(userEntity)).Returns((returnedToken, HandlerTest.TestTokenTime));
    }

    /// <summary>
    /// Setups <see cref="ISecretService.GenerateToken(Guid, TimeSpan)"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="returnedToken">The returned token.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupGenerateToken(this Mock<ISecretService> mock, string returnedToken = HandlerTest.TestToken)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.GenerateToken(It.IsAny<Guid>(), It.IsAny<TimeSpan>())).Returns((returnedToken, HandlerTest.TestTokenTime));
    }
    /// <summary>
    /// Setups <see cref="ISecretService.HashPassword(string)"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="password">The test password.</param>
    /// <param name="returns">The returned value.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupHashPassword(this Mock<ISecretService> mock, string password = HandlerTest.TestPassword, string returns = HandlerTest.TestHashedPassword)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.HashPassword(password)).Returns(returns);
    }

    /// <summary>
    /// Setups <see cref="ISecretService.VerifyPassword(string, string)"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="hash">The test hash.</param>
    /// <param name="password">The test password.</param>
    /// <param name="result">The returned value.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupVerifyPassword(this Mock<ISecretService> mock, string hash, string password, bool result)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.VerifyPassword(hash, password)).Returns(result);
    }
}
