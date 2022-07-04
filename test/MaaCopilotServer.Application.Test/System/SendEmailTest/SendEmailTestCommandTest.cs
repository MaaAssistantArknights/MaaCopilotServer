// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;
using MaaCopilotServer.Application.System.SendEmailTest;

namespace MaaCopilotServer.Application.Test.System.SendEmailTest;

/// <summary>
/// Tests <see cref="SendEmailTestCommandHandler"/>.
/// </summary>
[TestClass]
public class SendEmailTestCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler.Handle(SendEmailTestCommand, CancellationToken)"/>
    /// with wrong token.
    /// </summary>
    [TestMethod]
    public void TestHandle_InvalidToken()
    {
        var response = new HandlerTest()
            .TestSendEmailTest(new()
            {
                Token = "wrong_token",
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler.Handle(SendEmailTestCommand, CancellationToken)"/>
    /// with email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandle_FailedToSend()
    {
        var response = new HandlerTest()
            .SetupSendEmailAsync(false)
            .TestSendEmailTest(new()
            {
                Token = HandlerTest.TestToken,
                TargetAddress = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler.Handle(SendEmailTestCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var response = new HandlerTest()
            .SetupSendEmailAsync(true)
            .TestSendEmailTest(new()
            {
                Token = HandlerTest.TestToken,
                TargetAddress = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
