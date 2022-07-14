// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Test.TestEntities;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.RegisterCopilotAccount;

/// <summary>
/// Tests <see cref="RegisterCopilotAccountCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class RegisterCopilotAccountCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with email in use.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailInUse()
    {
        var user = new CopilotUserFactory().Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));

        var result = test.TestRegisterCopilotAccount(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailFailedToSend()
    {
        var test = new HandlerTest();
        test.SecretService.SetupHashPassword();
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(false);

        var result = test.TestRegisterCopilotAccount(new()
        {
            Email = HandlerTest.TestEmail,
            UserName = HandlerTest.TestUsername,
            Password = HandlerTest.TestPassword,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        result.DbContext.CopilotUsers.Should().BeEmpty();
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var test = new HandlerTest();
        test.SecretService.SetupHashPassword();
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(true);

        var result = test.TestRegisterCopilotAccount(new()
        {
            Email = HandlerTest.TestEmail,
            UserName = HandlerTest.TestUsername,
            Password = HandlerTest.TestPassword,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

        var token = result.DbContext.CopilotTokens.FirstOrDefault();
        token.Should().NotBeNull();
        token!.Type.Should().Be(Domain.Enums.TokenType.UserActivation);
        token.Token.Should().Be(HandlerTest.TestToken);

        var userEntity = token.ResourceId;
        var user = result.DbContext.CopilotUsers.FirstOrDefault();
        user.Should().NotBeNull();
        user!.EntityId.Should().Be(userEntity);
        user.Email.Should().Be(HandlerTest.TestEmail);
        user.UserName.Should().Be(HandlerTest.TestUsername);
        user.Password.Should().Be(HandlerTest.TestHashedPassword);
        user.UserRole.Should().Be(Domain.Enums.UserRole.User);
        user.UserActivated.Should().BeFalse();
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with default role higher than <see cref="Domain.Enums.UserRole.Uploader"/>.
    /// </summary>
    [TestMethod]
    public void TestHandleDefaultRoleTooHigh()
    {
        var test = new HandlerTest();
        test.SecretService.SetupHashPassword();
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(true);
        test.CopilotServerOption = new()
        {
            RegisterUserDefaultRole = Domain.Enums.UserRole.Admin,
        };

        var result = test.TestRegisterCopilotAccount(new()
        {
            Email = HandlerTest.TestEmail,
            UserName = HandlerTest.TestUsername,
            Password = HandlerTest.TestPassword,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);

        var token = result.DbContext.CopilotTokens.FirstOrDefault();
        token.Should().NotBeNull();
        token!.Type.Should().Be(Domain.Enums.TokenType.UserActivation);
        token.Token.Should().Be(HandlerTest.TestToken);

        var userEntity = token.ResourceId;
        var user = result.DbContext.CopilotUsers.FirstOrDefault();
        user.Should().NotBeNull();
        user!.EntityId.Should().Be(userEntity);
        user.Email.Should().Be(HandlerTest.TestEmail);
        user.UserName.Should().Be(HandlerTest.TestUsername);
        user.Password.Should().Be(HandlerTest.TestHashedPassword);
        user.UserRole.Should().Be(Domain.Enums.UserRole.Uploader);
        user.UserActivated.Should().BeFalse();
    }
}
