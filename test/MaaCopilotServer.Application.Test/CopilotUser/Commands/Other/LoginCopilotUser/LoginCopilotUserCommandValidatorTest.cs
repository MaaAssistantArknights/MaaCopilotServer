// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MaaCopilotServer.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Other.LoginCopilotUser;

/// <summary>
/// Tests <see cref="LoginCopilotUserCommandValidator"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class LoginCopilotUserCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="LoginCopilotUserCommandValidator"/>.
    /// </summary>
    /// <param name="email">The test email.</param>
    /// <param name="password">The test password.</param>
    /// <param name="expected">The expected validation result.</param>
    [DataTestMethod]
    [DataRow("test@example.com", "password", true)]
    [DataRow(null, "password", false)]
    [DataRow("invalid_email", "password", false)]
    [DataRow("test@example.com", null, false)]
    [DataRow("test@example.com", "pass", false)]
    [DataRow("test@example.com", "0123456789012345678901234567890123456789", false)]
    public void Test(string? email, string? password, bool expected)
    {
        ValidatorTestHelper.Test<LoginCopilotUserCommandValidator, LoginCopilotUserCommand>(new()
        {
            Email = email,
            Password = password,
        }, expected);
    }
}
