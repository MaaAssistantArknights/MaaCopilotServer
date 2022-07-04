// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.RequestPasswordReset;

/// <summary>
/// Tests <see cref="RequestPasswordResetCommandValidator"/>.
/// </summary>
[TestClass]
public class RequestPasswordResetCommandValidatorTest
{
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
        ValidatorTestHelper.Test<RequestPasswordResetCommandValidator, RequestPasswordResetCommand>(
            new()
            {
                Email = email,
            }, expected);
    }
}
