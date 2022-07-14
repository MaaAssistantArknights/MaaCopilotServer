// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Test.TestEntities;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Other.LoginCopilotUser;

/// <summary>
/// Tests <see cref="LoginCopilotUserCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class LoginCopilotUserCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="LoginCopilotUserCommandHandler.Handle(LoginCopilotUserCommand, CancellationToken)"/>
    /// with user not found.
    /// </summary>
    [TestMethod]
    public void TestHandleUserNotFound()
    {
        var test = new HandlerTest();

        var result = test.TestLoginCopilotUser(new()
        {
            Email = HandlerTest.TestEmail,
            Password = HandlerTest.TestPassword,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="LoginCopilotUserCommandHandler.Handle(LoginCopilotUserCommand, CancellationToken)"/>
    /// with wrong password.
    /// </summary>
    [TestMethod]
    public void TestHandleWrongPassword()
    {
        var user = new CopilotUserFactory { Email = HandlerTest.TestEmail }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.SecretService.SetupVerifyPassword(string.Empty, HandlerTest.TestPassword, false);

        var result = test.TestLoginCopilotUser(new()
        {
            Email = HandlerTest.TestEmail,
            Password = HandlerTest.TestPassword,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="LoginCopilotUserCommandHandler.Handle(LoginCopilotUserCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new CopilotUserFactory { Email = HandlerTest.TestEmail, Password = HandlerTest.TestPassword }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.SecretService.SetupVerifyPassword(HandlerTest.TestHashedPassword, HandlerTest.TestPassword, true);
        test.SecretService.SetupGenerateJwtToken(user.EntityId);

        var result = test.TestLoginCopilotUser(new()
        {
            Email = HandlerTest.TestEmail,
            Password = HandlerTest.TestPassword,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        ((LoginCopilotUserDto)result.Response.Data!).Token.Should().Be(HandlerTest.TestToken);
    }
}
