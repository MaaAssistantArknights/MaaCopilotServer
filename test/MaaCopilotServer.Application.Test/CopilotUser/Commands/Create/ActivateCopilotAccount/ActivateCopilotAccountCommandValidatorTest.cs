// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.ActivateCopilotAccount;

/// <summary>
/// Tests of <see cref="ActivateCopilotAccountCommandValidator"/>.
/// </summary>
[TestClass]
public class ActivateCopilotAccountCommandValidatorTest
{
    /// <summary>
    /// The validation error message.
    /// </summary>
    private readonly Resources.ValidationErrorMessage _validationErrorMessage = new();

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandValidator"/>.
    /// </summary>
    /// <param name="token">The test token.</param>
    /// <param name="expected">The expected valiation result.</param>
    [DataTestMethod]
    [DataRow("token", true)]
    [DataRow(null, false)]
    public void Test(string? token, bool expected)
    {
        var validator = new ActivateCopilotAccountCommandValidator(_validationErrorMessage);
        var data = new ActivateCopilotAccountCommand()
        {
            Token = token,
        };

        var result = validator.Validate(data);
        result.IsValid.Should().Be(expected);
    }
}
