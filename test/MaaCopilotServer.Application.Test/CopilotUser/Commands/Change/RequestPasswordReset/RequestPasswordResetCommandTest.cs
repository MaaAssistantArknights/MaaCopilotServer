// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Test.TestEntities;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.RequestPasswordReset;

/// <summary>
/// Tests <see cref="RequestPasswordResetCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class RequestPasswordResetCommandTest
{
    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with non-existing email.
    /// </summary>
    [TestMethod]
    public void TestHandleUserNotFound()
    {
        var result = new HandlerTest().TestRequestPasswordReset(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with the case when the email is not sent successfully.
    /// </summary>
    [TestMethod]
    public void TestHandleSendingEmailFailed()
    {
        var user = new CopilotUserFactory { Email = HandlerTest.TestEmail }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(false);

        var result = test.TestRequestPasswordReset(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with the successful case.
    /// </summary>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestHandleSuccessful(bool alreadyHaveToken)
    {
        var user = new CopilotUserFactory { Email = HandlerTest.TestEmail }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.DbContext.Setup(db =>
        {
            if (alreadyHaveToken)
            {
                var oldToken = new CopilotTokenFactory { ResourceId = user.EntityId, Type = TokenType.UserPasswordReset, Token = "old_token", ValidBefore = new() }.Build();
                db.CopilotTokens.Add(oldToken);
            }
        });
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(true);

        var result = test.TestRequestPasswordReset(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var token = result.DbContext.CopilotTokens.FirstOrDefault(
            x => x.ResourceId == user.EntityId && x.Type == TokenType.UserPasswordReset);
        token.Should().NotBeNull();
        token!.Token.Should().Be(HandlerTest.TestToken);
    }
}
