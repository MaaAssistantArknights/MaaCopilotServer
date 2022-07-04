// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.System.SendEmailTest;
using MaaCopilotServer.Application.Test.TestHelpers;

namespace MaaCopilotServer.Application.Test.System.SendEmailTest;

/// <summary>
/// Tests <see cref="SendEmailTestCommandValidator"/>.
/// </summary>
[TestClass]
public class SendEmailTestCommandValidatorTest
{
    /// <summary>
    /// Tests <see cref="SendEmailTestCommandValidator"/>.
    /// </summary>
    /// <param name="targetAddress">The test target address.</param>
    /// <param name="expected">The expected validation result.</param>
    [DataTestMethod]
    [DataRow("user@example.com", true)]
    [DataRow(null, false)]
    [DataRow("invalid_email", false)]
    public void Test(string? targetAddress, bool expected)
    {
        ValidatorTestHelper.Test<SendEmailTestCommandValidator, SendEmailTestCommand>(new()
        {
            TargetAddress = targetAddress
        }, expected);
    }
}
