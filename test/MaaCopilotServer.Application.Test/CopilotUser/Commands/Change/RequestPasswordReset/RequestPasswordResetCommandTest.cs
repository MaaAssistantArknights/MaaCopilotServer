// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.RequestPasswordReset;

/// <summary>
/// Tests for <see cref="RequestPasswordResetCommandHandler"/>.
/// </summary>
[TestClass]
public class RequestPasswordResetCommandTest
{
    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with non-existing email.
    /// </summary>
    [TestMethod]
    public void TestHandle_UserNotFound()
    {
        var response = new HandlerTest()
            .TestRequestPasswordReset(new()
            {
                Email = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with the case when the email is not sent successfully.
    /// </summary>
    [TestMethod]
    public void TestHandle_SendingEmailFailed()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGenerateToken()
            .SetupSendEmailAsync(false)
            .TestRequestPasswordReset(new()
            {
                Email = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="RequestPasswordResetCommandHandler.Handle(RequestPasswordResetCommand, CancellationToken)"/> with the successful case.
    /// </summary>
    [DataTestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public void TestHandle_Successful(bool alreadyHaveToken)
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db =>
            {
                if (alreadyHaveToken)
                {
                    var oldToken = new Domain.Entities.CopilotToken(user.EntityId, Domain.Enums.TokenType.UserPasswordReset, "old_token", new DateTimeOffset());
                    db.CopilotTokens.Add(oldToken);
                }
            })
            .SetupGenerateToken()
            .SetupSendEmailAsync(true).TestRequestPasswordReset(new()
            {
                Email = HandlerTest.TestEmail,
            });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var token = result.DbContext.CopilotTokens.FirstOrDefault(
            x => x.ResourceId == user.EntityId && x.Type == Domain.Enums.TokenType.UserPasswordReset);
        token.Should().NotBeNull();
        token!.Token.Should().Be(HandlerTest.TestToken);
    }
}
