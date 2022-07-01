// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.ActivateCopilotAccount;

/// <summary>
/// Tests of <see cref="ActivateCopilotAccountCommandValidator"/>.
/// </summary>
[TestClass]
public class ActivateCopilotAccountCommandValidatorTest
{
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
        ValidatorTestHelper.Test<ActivateCopilotAccountCommandValidator, ActivateCopilotAccountCommand>(
            new()
            {
                Token = token,
            }, expected);
    }
}
