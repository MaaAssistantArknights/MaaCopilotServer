// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Email.Models;

namespace MaaCopilotServer.Application.Test.TestExtensions;

/// <summary>
/// Test extension for <see cref="IMailService"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class MailServiceTestExtension
{
    /// <summary>
    /// Setups <see cref="IMailService.SendEmailAsync{T}(T, string)"/>.
    /// </summary>
    /// <param name="mock">The mock object.</param>
    /// <param name="success">Whether the email sending is successful.</param>
    /// <param name="targetAddress">The test target address.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="mock"/> is <c>null</c>.</exception>
    public static void SetupSendEmailAsync(this Mock<IMailService> mock, bool success, string targetAddress = HandlerTest.TestEmail)
    {
        if (mock == null)
        {
            throw new ArgumentNullException(nameof(mock));
        }

        mock.Setup(x => x.SendEmailAsync(It.IsAny<IEmailModel>(), targetAddress)).ReturnsAsync(success);
    }
}
