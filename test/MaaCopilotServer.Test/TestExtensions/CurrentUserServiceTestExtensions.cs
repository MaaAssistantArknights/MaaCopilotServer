// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Interfaces;
using Moq;

namespace MaaCopilotServer.Application.Test.TestExtensions;

/// <summary>
/// Test extension for <see cref="ICurrentUserService"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class CurrentUserServiceTestExtensions
{

    /// <summary>
    /// Setups <see cref="ICurrentUserService.GetUser"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="returns">The returned value.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupGetUser(this Mock<ICurrentUserService> mock, Domain.Entities.CopilotUser? returns)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.GetUser().Result).Returns(returns);
    }
    /// <summary>
    /// Setups <see cref="ICurrentUserService.GetUserIdentity"/>
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="returns">The returned value.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupGetUserIdentity(this Mock<ICurrentUserService> mock, Guid? returns)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.GetUserIdentity()).Returns(returns);
    }
}
