// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.RegisterCopilotAccount;

/// <summary>
/// Tests of <see cref="RegisterCopilotAccountCommandValidator"/>.
/// </summary>
[TestClass]
public class RegisterCopilotAccountCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandValidator"/>.
    /// </summary>
    /// <param name="email">The test email.</param>
    /// <param name="password">The test password.</param>
    /// <param name="username">The test username.</param>
    /// <param name="expected">The expected valiation result.</param>
    [DataTestMethod]
    [DataRow("user@example.com", "password", "username", true)]
    [DataRow(null, "password", "username", false)]
    [DataRow("invalid_email", "password", "username", false)]
    [DataRow("user@example.com", null, "username", false)]
    [DataRow("user@example.com", "pass", "username", false)]
    [DataRow("user@example.com", "0123456789012345678901234567890123456789", "username", false)]
    [DataRow("user@example.com", "password", null, false)]
    [DataRow("user@example.com", "password", "u", false)]
    [DataRow("user@example.com", "password", "012345678901234567890123456789", false)]
    public void Test(string? email, string? password, string? username, bool expected)
    {
        ValidatorTestHelper.Test<RegisterCopilotAccountCommandValidator, RegisterCopilotAccountCommand>(
            new()
            {
                Email = email,
                Password = password,
                UserName = username,
            }, expected);
    }
}
