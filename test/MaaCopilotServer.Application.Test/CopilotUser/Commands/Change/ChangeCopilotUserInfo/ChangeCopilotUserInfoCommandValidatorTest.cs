// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.ChangeCopilotUserInfo;

/// <summary>
/// Tests <see cref="ChangeCopilotUserInfoCommandValidator"/>.
/// </summary>
[TestClass]
public class ChangeCopilotUserInfoCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandValidator"/>.
    /// </summary>
    /// <param name="userId">The test user ID.</param>
    /// <param name="email">The test email.</param>
    /// <param name="password">The test password.</param>
    /// <param name="username">The test username.</param>
    /// <param name="role">The test role.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "user@example.com", "password", "user", "User", true)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", null, null, null, "User", true)]
    [DataRow(null, "user@example.com", "password", "user", "User", false)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "invalid_email", "password", "user", "User", false)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "user@example.com", "pass", "user", "User", false)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "user@example.com", "0123456789012345678901234567890123456789", "user", "User", false)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "user@example.com", "password", "u", "User", false)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "user@example.com", "password", "012345678901234567890123456789", "User", false)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "user@example.com", "password", "user", "invalid_role", false)]
    [DataRow("382c74c3-721d-4f34-80e5-57657b6cbc27", "user@example.com", "password", "user", "SuperAdmin", false)]
    public void Test(string? userId, string? email, string? password, string? username, string? role, bool expected)
    {
        ValidatorTestHelper.Test<ChangeCopilotUserInfoCommandValidator, ChangeCopilotUserInfoCommand>(
            new()
            {
                UserId = userId,
                Email = email,
                Password = password,
                UserName = username,
                Role = role,
            }, expected);
    }
}
