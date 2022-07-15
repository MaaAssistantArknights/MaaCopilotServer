// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.CreateCopilotUser;

/// <summary>
/// Tests <see cref="CreateCopilotUserCommandValidator"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CreateCopilotUserCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandValidator"/>.
    /// </summary>
    /// <param name="email">The test email.</param>
    /// <param name="password">The test password.</param>
    /// <param name="username">The test username.</param>
    /// <param name="role">The test role.</param>
    /// <param name="expected">The expected valiation result.</param>
    [DataTestMethod]
    [DataRow("user@example.com", "password", "username", "User", true)]
    [DataRow(null, "password", "username", "User", false)]
    [DataRow("invalid_email", "password", "username", "User", false)]
    [DataRow("user@example.com", null, "username", "User", false)]
    [DataRow("user@example.com", "pass", "username", "User", false)]
    [DataRow("user@example.com", "0123456789012345678901234567890123456789", "username", "User", false)]
    [DataRow("user@example.com", "password", null, "User", false)]
    [DataRow("user@example.com", "password", "u", "User", false)]
    [DataRow("user@example.com", "password", "012345678901234567890123456789", "User", false)]
    [DataRow("user@example.com", "password", "username", null, false)]
    [DataRow("user@example.com", "password", "username", "invalid_role", false)]
    [DataRow("user@example.com", "password", "username", "SuperAdmin", false)]
    public void Test(string? email, string? password, string? username, string? role, bool expected)
    {
        ValidatorTestHelper.Test<CreateCopilotUserCommandValidator, CreateCopilotUserCommand>(
            new()
            {
                Email = email,
                Password = password,
                UserName = username,
                Role = role,
            }, expected);
    }
}
