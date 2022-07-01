// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Create.CreateCopilotUser;

/// <summary>
/// Tests of <see cref="CreateCopilotUserCommandHandler"/>.
/// </summary>
[TestClass]
public class CreateCopilotUserCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler.Handle(CreateCopilotUserCommand, CancellationToken)"/> with email in use.
    /// </summary>
    [TestMethod]
    public void TestHandle_EmailInUse()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .TestCreateCopilotUser(new()
            {
                Email = HandlerTest.TestEmail,
            });

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="CreateCopilotUserCommandHandler.Handle(CreateCopilotUserCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var adminUserEntity = Guid.NewGuid();

        var test = new HandlerTest()
                    .SetupHashPassword()
                    .SetupGetUserIdentity(adminUserEntity);
        var response = test.TestCreateCopilotUser(new()
        {
            Email = HandlerTest.TestEmail,
            Password = HandlerTest.TestPassword,
            UserName = HandlerTest.TestUsername,
            Role = "User",
        });

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var user = test.DbContext.CopilotUsers.FirstOrDefault(x => x.Email == HandlerTest.TestEmail);
        user.Should().NotBeNull();
        user!.Password.Should().Be(HandlerTest.TestHashedPassword);
        user.UserName.Should().Be(HandlerTest.TestUsername);
        user.UserRole.Should().Be(Domain.Enums.UserRole.User);
        user.UserActivated.Should().BeTrue();
    }
}
