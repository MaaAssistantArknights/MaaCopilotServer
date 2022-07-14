// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.System.SendEmailTest;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.System.SendEmailTest;

/// <summary>
/// Tests <see cref="SendEmailTestCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class SendEmailTestCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler.Handle(SendEmailTestCommand, CancellationToken)"/>
    /// with wrong token.
    /// </summary>
    [TestMethod]
    public void TestHandleInvalidToken()
    {
        var test = new HandlerTest();

        var result = test.TestSendEmailTest(new()
        {
            Token = "wrong_token",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler.Handle(SendEmailTestCommand, CancellationToken)"/>
    /// with email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandleFailedToSend()
    {
        var test = new HandlerTest();
        test.MailService.SetupSendEmailAsync(false);

        var result = test.TestSendEmailTest(new()
        {
            Token = HandlerTest.TestToken,
            TargetAddress = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="SendEmailTestCommandHandler.Handle(SendEmailTestCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var test = new HandlerTest();
        test.MailService.SetupSendEmailAsync(true);

        var result = test.TestSendEmailTest(new()
        {
            Token = HandlerTest.TestToken,
            TargetAddress = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}
