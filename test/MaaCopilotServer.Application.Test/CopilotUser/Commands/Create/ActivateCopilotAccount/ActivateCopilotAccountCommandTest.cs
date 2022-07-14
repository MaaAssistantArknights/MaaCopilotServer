// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Test.TestEntities;
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
        var token = new CopilotTokenFactory { Type = TokenType.UserActivation, Token = HandlerTest.TestToken, ValidBefore = HandlerTest.TestTokenTimePast }.Build();

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
        var token = new CopilotTokenFactory { Type = TokenType.UserPasswordReset, Token = HandlerTest.TestToken, ValidBefore = HandlerTest.TestTokenTimeFuture }.Build();

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
        var token = new CopilotTokenFactory { Type = TokenType.UserActivation, Token = HandlerTest.TestToken, ValidBefore = HandlerTest.TestTokenTimeFuture }.Build();

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
        var user = new CopilotUserFactory().Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        var token = new CopilotTokenFactory { ResourceId = user.EntityId, Type = TokenType.UserActivation, Token = HandlerTest.TestToken, ValidBefore = HandlerTest.TestTokenTimeFuture }.Build();
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
