// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.PasswordReset;

/// <summary>
/// Tests <see cref="PasswordResetCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class PasswordResetCommandTest
{
    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with invalid token.
    /// </summary>
    [TestMethod]
    public void TestHandleInvalidToken()
    {
        var response = new HandlerTest()
            .TestPasswordReset(new()
            {
                Token = HandlerTest.TestToken,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with expired token.
    /// </summary>
    [TestMethod]
    public void TestHandleExpiredToken()
    {
        var token = new CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserPasswordReset, HandlerTest.TestToken, HandlerTest.TestTokenTimePast);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotTokens.Add(token))
            .TestPasswordReset(new()
            {
                Token = HandlerTest.TestToken,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with token of wrong type.
    /// </summary>
    [TestMethod]
    public void TestHandleWrongTypeToken()
    {
        var token = new CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserActivation, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotTokens.Add(token))
            .TestPasswordReset(new()
            {
                Token = HandlerTest.TestToken,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with invalid user.
    /// </summary>
    [TestMethod]
    public void TestHandleInvalidUser()
    {
        var token = new CopilotToken(Guid.Empty, Domain.Enums.TokenType.UserPasswordReset, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotTokens.Add(token))
            .TestPasswordReset(new()
            {
                Token = HandlerTest.TestToken,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="PasswordResetCommandHandler.Handle(PasswordResetCommand, CancellationToken)"/> with valid data.
    /// </summary>
    [TestMethod]
    public void TestHandleValid()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db =>
            {
                var token = new CopilotToken(user.EntityId, Domain.Enums.TokenType.UserPasswordReset, HandlerTest.TestToken, HandlerTest.TestTokenTimeFuture);
                db.CopilotTokens.Add(token);
            })
            .SetupHashPassword()
            .TestPasswordReset(new()
            {
                Token = HandlerTest.TestToken,
                Password = HandlerTest.TestPassword,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Password.Should().Be("hashed_password");
    }
}
