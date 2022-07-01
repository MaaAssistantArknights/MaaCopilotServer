// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.UpdateCopilotUserInfo;

/// <summary>
/// Tests for <see cref="UpdateCopilotUserInfoCommandValidator"/>.
/// </summary>
[TestClass]
public class UpdateCopilotUserInfoCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandValidator"/>.
    /// </summary>
    /// <param name="email">The test email.</param>
    /// <param name="username">The test username.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow("user@example.com", "username", true)]
    [DataRow(null, "username", true)]
    [DataRow("user@example.com", null, true)]
    [DataRow("invalid_email", null, false)]
    [DataRow(null, "u", false)]
    [DataRow(null, "012345678901234567890123456789", false)]
    public void Test(string? email, string? username, bool expected)
    {
        ValidatorTestHelper.Test<UpdateCopilotUserInfoCommandValidator, UpdateCopilotUserInfoCommand>(
            new()
            {
                Email = email,
                UserName = username,
            }, expected);
    }
}
