// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.ActivateCopilotAccount;

/// <summary>
/// Tests of <see cref="ActivateCopilotAccountCommandHandler"/>.
/// </summary>
[TestClass]
public class ActivateCopilotAccountCommandTest
{
    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with not existing token.
    /// </summary>
    [TestMethod]
    public void TestHandle_NotExistingToken()
    {
        var response = new HandlerTest()
            .TestActivateCopilotAccount(new()
            {
                Token = HandlerTest.TestToken,
            });

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with expired token.
    /// </summary>
    [TestMethod]
    public void TestHandle_ExpiredToken()
    {
        var token = new CopilotToken(Guid.Empty, TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimePast);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotTokens.Add(token))
            .TestActivateCopilotAccount(new()
            {
                Token = HandlerTest.TestToken
            });

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with token of wrong type.
    /// </summary>
    [TestMethod]
    public void TestHandle_WrongTypeToken()
    {
        var token = new CopilotToken(Guid.Empty, TokenType.UserPasswordReset, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotTokens.Add(token))
            .TestActivateCopilotAccount(new()
            {
                Token = HandlerTest.TestToken
            });

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with user not found.
    /// </summary>
    [TestMethod]
    public void TestHandle_UserNotFound()
    {
        var token = new CopilotToken(Guid.Empty, TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotTokens.Add(token))
            .TestActivateCopilotAccount(new()
            {
                Token = HandlerTest.TestToken
            });

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, null);

        var test = new HandlerTest()
                    .SetupDatabase(db => db.CopilotUsers.Add(user))
                    .SetupDatabase(db => db.CopilotTokens.Add(new(user.EntityId, TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture)));
        var response = test.TestActivateCopilotAccount(new()
        {
            Token = HandlerTest.TestToken,
        });

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserActivated.Should().BeTrue();
        test.DbContext.CopilotTokens.FirstOrDefault(x => x.Token == HandlerTest.TestToken).Should().BeNull();
    }
}
