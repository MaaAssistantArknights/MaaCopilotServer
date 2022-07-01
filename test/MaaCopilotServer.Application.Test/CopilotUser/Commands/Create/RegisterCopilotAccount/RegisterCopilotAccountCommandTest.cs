// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.RegisterCopilotAccount;

/// <summary>
/// Tests of <see cref="RegisterCopilotAccountCommandHandler"/>.
/// </summary>
[TestClass]
public class RegisterCopilotAccountCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with email in use.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailInUse()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);

        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .TestRegisterCopilotAccount(new()
            {
                Email = HandlerTest.TestEmail,
            });

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>
    /// with email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailFailedToSend()
    {
        var test = new HandlerTest()
            .SetupHashPassword()
            .SetupGenerateToken()
            .SetupSendEmailAsync(false);
        var response = test.TestRegisterCopilotAccount(new()
        {
            Email = HandlerTest.TestEmail,
            UserName = HandlerTest.TestUsername,
            Password = HandlerTest.TestPassword,
        });

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        test.DbContext.CopilotUsers.Should().BeEmpty();
    }

    /// <summary>
    /// Tests <see cref="RegisterCopilotAccountCommandHandler.Handle(RegisterCopilotAccountCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var test = new HandlerTest()
            .SetupHashPassword()
            .SetupGenerateToken()
            .SetupSendEmailAsync(true);
        var response = test.TestRegisterCopilotAccount(new()
        {
            Email = HandlerTest.TestEmail,
            UserName = HandlerTest.TestUsername,
            Password = HandlerTest.TestPassword,
        });

        response.StatusCode.Should().Be(StatusCodes.Status200OK);

        var token = test.DbContext.CopilotTokens.FirstOrDefault();
        token.Should().NotBeNull();
        token!.Type.Should().Be(Domain.Enums.TokenType.UserActivation);
        token.Token.Should().Be(HandlerTest.TestToken);

        var userEntity = token.ResourceId;
        var user = test.DbContext.CopilotUsers.FirstOrDefault();
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
    public void TestHandle_DefaultRoleTooHigh()
    {
        var test = new HandlerTest()
            .SetupHashPassword()
            .SetupGenerateToken()
            .SetupSendEmailAsync(true)
            .SetupCopilotServerOption(new()
            {
                RegisterUserDefaultRole = Domain.Enums.UserRole.Admin,
            });
        var response = test.TestRegisterCopilotAccount(new()
        {
            Email = HandlerTest.TestEmail,
            UserName = HandlerTest.TestUsername,
            Password = HandlerTest.TestPassword,
        });

        response.StatusCode.Should().Be(StatusCodes.Status200OK);

        var token = test.DbContext.CopilotTokens.FirstOrDefault();
        token.Should().NotBeNull();
        token!.Type.Should().Be(Domain.Enums.TokenType.UserActivation);
        token.Token.Should().Be(HandlerTest.TestToken);

        var userEntity = token.ResourceId;
        var user = test.DbContext.CopilotUsers.FirstOrDefault();
        user.Should().NotBeNull();
        user!.EntityId.Should().Be(userEntity);
        user.Email.Should().Be(HandlerTest.TestEmail);
        user.UserName.Should().Be(HandlerTest.TestUsername);
        user.Password.Should().Be(HandlerTest.TestHashedPassword);
        user.UserRole.Should().Be(Domain.Enums.UserRole.Uploader);
        user.UserActivated.Should().BeFalse();
    }
}
