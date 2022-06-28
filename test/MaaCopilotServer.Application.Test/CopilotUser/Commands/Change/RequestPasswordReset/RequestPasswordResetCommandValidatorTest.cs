// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.RequestPasswordReset;

/// <summary>
/// Tests for <see cref="RequestPasswordResetCommandValidator"/>.
/// </summary>
[TestClass]
public class RequestPasswordResetCommandValidatorTest
{
    /// <summary>
    /// The validation error message.
    /// </summary>
    private readonly Resources.ValidationErrorMessage _validationErrorMessage = new();

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandValidator"/>.
    /// </summary>
    /// <param name="email">The test email.</param>
    /// <param name="expected">The expected valiation result.</param>
    [DataTestMethod]
    [DataRow("user@example.com", true)]
    [DataRow(null, false)]
    [DataRow("invalid_email", false)]
    public void Test(string? email, bool expected)
    {
        var validator = new RequestPasswordResetCommandValidator(_validationErrorMessage);
        var data = new RequestPasswordResetCommand()
        {
            Email = email,
        };

        var result = validator.Validate(data);
        result.IsValid.Should().Be(expected);
    }
}
