// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.RequestActivationToken;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Other.RequestActivationToken;

/// <summary>
/// Tests <see cref="RequestActivationTokenCommandHandler"/>.
/// </summary>
[TestClass]
public class RequestActivationTokenCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>
    /// with user already activated.
    /// </summary>
    [TestMethod]
    public void TestHandle_UserAlreadyActivated()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        user.ActivateUser(Guid.Empty);

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>
    /// with token not found.
    /// </summary>
    [TestMethod]
    public void TestHandle_TokenNotFound()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotTokens.Add(
                new Domain.Entities.CopilotToken(user.EntityId, Domain.Enums.TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture)))
            .SetupGetUser(user)
            .TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>
    /// with email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailFailedToSend()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .SetupGenerateToken()
            .SetupSendEmailAsync(false)
            .TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="RequestActivationTokenCommandHandler.Handle(RequestActivationTokenCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .SetupGenerateToken()
            .SetupSendEmailAsync(true)
            .TestRequestActivationToken(new());

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.DbContext.CopilotTokens
            .Where(x => x.Type == Domain.Enums.TokenType.UserActivation)
            .Where(x => x.ResourceId == user.EntityId)
            .Count()
            .Should()
            .Be(1);
    }
}
