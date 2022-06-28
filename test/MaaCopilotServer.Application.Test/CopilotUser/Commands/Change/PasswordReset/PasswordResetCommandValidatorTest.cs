// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;
using MaaCopilotServer.Resources;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.PasswordReset;

/// <summary>
/// Tests for <see cref="PasswordResetCommandValidator"/>.
/// </summary>
[TestClass]
public class PasswordResetCommandValidatorTest
{
    /// <summary>
    /// The validation error message.
    /// </summary>
    private readonly ValidationErrorMessage _validationErrorMessage = new();

    /// <summary>
    /// Tests <see cref="PasswordResetCommandValidator"/>.
    /// </summary>
    /// <param name="token">The test token.</param>
    /// <param name="password">The test password.</param>
    /// <param name="expected">The expected valiation result.</param>
    [DataTestMethod]
    [DataRow("token", "password", true)]
    [DataRow(null, "password", false)]
    [DataRow("token", null, false)]
    [DataRow("token", "pass", false)]
    [DataRow("token", "0123456789012345678901234567890123456789", false)]
    public void Test(string? token, string? password, bool expected)
    {
        var validator = new PasswordResetCommandValidator(_validationErrorMessage);
        var data = new PasswordResetCommand()
        {
            Token = token,
            Password = password,
        };

        var result = validator.Validate(data);
        result.IsValid.Should().Be(expected);
    }
}
