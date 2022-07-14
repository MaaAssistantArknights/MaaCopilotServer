// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.ActivateCopilotAccount;

/// <summary>
/// Tests <see cref="ActivateCopilotAccountCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class ActivateCopilotAccountCommandTest
{
    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with not existing token.
    /// </summary>
    [TestMethod]
    public void TestHandleNotExistingToken()
    {
        var result = new HandlerTest().TestActivateCopilotAccount(new()
        {
            Token = HandlerTest.TestToken,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with expired token.
    /// </summary>
    [TestMethod]
    public void TestHandleExpiredToken()
    {
        var token = new CopilotToken(Guid.Empty, TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimePast);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotTokens.Add(token));

        var result = test.TestActivateCopilotAccount(new()
        {
            Token = HandlerTest.TestToken
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with token of wrong type.
    /// </summary>
    [TestMethod]
    public void TestHandleWrongTypeToken()
    {
        var token = new CopilotToken(Guid.Empty, TokenType.UserPasswordReset, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotTokens.Add(token));

        var result = test.TestActivateCopilotAccount(new()
        {
            Token = HandlerTest.TestToken
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>
    /// with user not found.
    /// </summary>
    [TestMethod]
    public void TestHandleUserNotFound()
    {
        var token = new CopilotToken(Guid.Empty, TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotTokens.Add(token));

        var result = test.TestActivateCopilotAccount(new()
        {
            Token = HandlerTest.TestToken
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="ActivateCopilotAccountCommandHandler.Handle(ActivateCopilotAccountCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, null);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        var token = new CopilotToken(user.EntityId, TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);
        test.DbContext.Setup(db => db.CopilotTokens.Add(token));

        var result = test.TestActivateCopilotAccount(new()
        {
            Token = HandlerTest.TestToken,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserActivated.Should().BeTrue();
        result.DbContext.CopilotTokens.FirstOrDefault(x => x.Token == HandlerTest.TestToken).Should().BeNull();
    }
}
