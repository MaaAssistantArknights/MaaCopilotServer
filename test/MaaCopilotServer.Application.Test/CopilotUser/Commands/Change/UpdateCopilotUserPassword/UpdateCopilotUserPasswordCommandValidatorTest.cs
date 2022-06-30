// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.UpdateCopilotUserInfo;

/// <summary>
/// Tests for <see cref="UpdateCopilotUserPasswordCommandValidator"/>.
/// </summary>
[TestClass]
public class UpdateCopilotUserPasswordCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandValidator"/>.
    /// </summary>
    /// <param name="originalPassword">The test original password.</param>
    /// <param name="newPassword">The test new password.</param>
    /// <param name="expected">The expected result.</param>
    [DataTestMethod]
    [DataRow("password", "password", true)]
    [DataRow(null, "password", false)]
    [DataRow("password", null, false)]
    [DataRow("p", "password", false)]
    [DataRow("password", "p", false)]
    [DataRow("0123456789012345678901234567890123456789", "password", false)]
    [DataRow("password", "0123456789012345678901234567890123456789", false)]
    public void Test(string? originalPassword, string? newPassword, bool expected)
    {
        var validator = new UpdateCopilotUserPasswordCommandValidator(new Resources.ValidationErrorMessage());
        var data = new UpdateCopilotUserPasswordCommand()
        {
            OriginalPassword = originalPassword,
            NewPassword = newPassword,
        };
        var result = validator.Validate(data);
        result.IsValid.Should().Be(expected);
    }
}
