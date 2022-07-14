// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Test.TestEntities;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.CreateCopilotUser;

/// <summary>
/// Tests <see cref="CreateCopilotUserCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class CreateCopilotUserCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler.Handle(CreateCopilotUserCommand, CancellationToken)"/> with email in use.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailInUse()
    {
        var user = new CopilotUserFactory { Email = HandlerTest.TestEmail }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));

        var result = test.TestCreateCopilotUser(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler.Handle(CreateCopilotUserCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var adminUserEntity = Guid.NewGuid();

        var test = new HandlerTest();
        test.SecretService.SetupHashPassword();
        test.CurrentUserService.SetupGetUserIdentity(adminUserEntity);

        var result = test.TestCreateCopilotUser(new()
        {
            Email = HandlerTest.TestEmail,
            Password = HandlerTest.TestPassword,
            UserName = HandlerTest.TestUsername,
            Role = "User",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var user = result.DbContext.CopilotUsers.FirstOrDefault(x => x.Email == HandlerTest.TestEmail);
        user.Should().NotBeNull();
        user!.Password.Should().Be(HandlerTest.TestHashedPassword);
        user.UserName.Should().Be(HandlerTest.TestUsername);
        user.UserRole.Should().Be(Domain.Enums.UserRole.User);
        user.UserActivated.Should().BeTrue();
    }
}
