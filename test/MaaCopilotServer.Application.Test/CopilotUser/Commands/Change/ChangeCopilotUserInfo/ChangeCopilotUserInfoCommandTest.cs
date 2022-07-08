// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.ChangeCopilotUserInfo;

/// <summary>
/// Tests <see cref="ChangeCopilotUserInfoCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class ChangeCopilotUserInfoCommandTest
{
    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with non-existing user.
    /// </summary>
    [TestMethod]
    public void TestHandleUserNotFound()
    {
        var response = new HandlerTest()
            .TestChangeCopilotUserInfo(new()
            {
                UserId = Guid.Empty.ToString(),
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with insufficient operator permission.
    /// </summary>
    /// <param name="userRole">The test user role.</param>
    /// <param name="operatorRole">The test operator role.</param>
    [DataTestMethod]
    [DataRow(Domain.Enums.UserRole.Admin, Domain.Enums.UserRole.Admin)]
    [DataRow(Domain.Enums.UserRole.SuperAdmin, Domain.Enums.UserRole.Admin)]
    public void TestHandleOperatorPermissionDenied(Domain.Enums.UserRole userRole, Domain.Enums.UserRole operatorRole)
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, userRole, null);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, operatorRole, null);

        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotUsers.Add(@operator))
            .SetupGetUser(@operator)
            .TestChangeCopilotUserInfo(new()
            {
                UserId = user.EntityId.ToString(),
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with super admin role changes.
    /// </summary>
    [TestMethod]
    public void TestHandleSuperAdminRoleChanges()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotUsers.Add(@operator))
            .SetupGetUser(@operator)
            .TestChangeCopilotUserInfo(new()
            {
                UserId = user.EntityId.ToString(),
                Role = Domain.Enums.UserRole.Admin.ToString(),
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with email address in use.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailInUse()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.Admin, null);

        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotUsers.Add(@operator))
            .SetupGetUser(@operator)
            .TestChangeCopilotUserInfo(new()
            {
                UserId = user.EntityId.ToString(),
                Email = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with valid request.
    /// </summary>
    [TestMethod]
    public void TestHandleValid()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        user.ActivateUser(Guid.Empty);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);

        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotUsers.Add(@operator))
            .SetupGetUser(@operator)
            .SetupHashPassword()
            .TestChangeCopilotUserInfo(new()
            {
                UserId = user.EntityId.ToString(),
                Email = HandlerTest.TestEmail,
                UserName = HandlerTest.TestUsername,
                Password = HandlerTest.TestPassword,
                Role = Domain.Enums.UserRole.Uploader.ToString(),
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserActivated.Should().BeTrue();
        user.Email.Should().Be(HandlerTest.TestEmail);
        user.UserName.Should().Be(HandlerTest.TestUsername);
        user.Password.Should().Be(HandlerTest.TestHashedPassword);
        user.UserRole.Should().Be(Domain.Enums.UserRole.Uploader);
    }
}
