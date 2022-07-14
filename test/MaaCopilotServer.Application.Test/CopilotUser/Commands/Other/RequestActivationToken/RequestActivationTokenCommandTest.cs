// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestActivationToken;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Test.TestEntities;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Other.RequestActivationToken;

/// <summary>
/// Tests <see cref="RequestActivationTokenCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class RequestActivationTokenCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>
    /// with user already activated.
    /// </summary>
    [TestMethod]
    public void TestHandleUserAlreadyActivated()
    {
        var user = new CopilotUserFactory().Build();
        user.ActivateUser(Guid.Empty);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);

        var result = test.TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>
    /// with token not found.
    /// </summary>
    [TestMethod]
    public void TestHandleTokenNotFound()
    {
        var user = new CopilotUserFactory().Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        var token = new CopilotTokenFactory { ResourceId = user.EntityId, Type = TokenType.UserActivation }.Build();
        test.DbContext.Setup(db => db.CopilotTokens.Add(token));
        test.CurrentUserService.SetupGetUser(user);

        var result = test.TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>
    /// with email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailFailedToSend()
    {
        var user = new CopilotUserFactory { Email = HandlerTest.TestEmail }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(false);

        var result = test.TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new CopilotUserFactory { Email = HandlerTest.TestEmail }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(true);

        var result = test.TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.DbContext.CopilotTokens
            .Where(x => x.Type == TokenType.UserActivation)
            .Where(x => x.ResourceId == user.EntityId)
            .Count()
            .Should()
            .Be(1);
    }
}
