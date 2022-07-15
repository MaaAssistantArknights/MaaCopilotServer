// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Domain.Enums;
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
        var result = new HandlerTest().TestChangeCopilotUserInfo(new()
        {
            UserId = Guid.Empty.ToString(),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with insufficient operator permission.
    /// </summary>
    /// <param name="userRole">The test user role.</param>
    /// <param name="operatorRole">The test operator role.</param>
    [DataTestMethod]
    [DataRow(UserRole.Admin, UserRole.Admin)]
    [DataRow(UserRole.SuperAdmin, UserRole.Admin)]
    public void TestHandleOperatorPermissionDenied(UserRole userRole, UserRole operatorRole)
    {
        var user = new CopilotUserFactory { UserRole = userRole }.Build();
        var @operator = new CopilotUserFactory { UserRole = operatorRole }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotUsers.Add(@operator);
        });
        test.CurrentUserService.SetupGetUser(@operator);

        var result = test.TestChangeCopilotUserInfo(new()
        {
            UserId = user.EntityId.ToString(),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with super admin role changes.
    /// </summary>
    [TestMethod]
    public void TestHandleSuperAdminRoleChanges()
    {
        var user = new CopilotUserFactory { UserRole = UserRole.SuperAdmin }.Build();
        var @operator = new CopilotUserFactory { UserRole = UserRole.SuperAdmin }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotUsers.Add(@operator);
        });
        test.CurrentUserService.SetupGetUser(@operator);

        var result = test.TestChangeCopilotUserInfo(new()
        {
            UserId = user.EntityId.ToString(),
            Role = UserRole.Admin.ToString(),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with email address in use.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailInUse()
    {
        var user = new CopilotUserFactory().Build();
        var @operator = new CopilotUserFactory { UserRole = UserRole.Admin }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotUsers.Add(@operator);
        });
        test.CurrentUserService.SetupGetUser(@operator);

        var response = test.TestChangeCopilotUserInfo(new()
        {
            UserId = user.EntityId.ToString(),
            Email = HandlerTest.TestEmail,
        }).Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="ChangeCopilotUserInfoCommandHandler.Handle(ChangeCopilotUserInfoCommand, CancellationToken)"/> with valid request.
    /// </summary>
    [TestMethod]
    public void TestHandleValid()
    {
        var user = new CopilotUserFactory { Email = string.Empty, UserRole = UserRole.User }.Build();
        user.ActivateUser(Guid.Empty);
        var @operator = new CopilotUserFactory { Email = string.Empty, UserRole = UserRole.SuperAdmin }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotUsers.Add(@operator);
        });
        test.CurrentUserService.SetupGetUser(@operator);
        test.SecretService.SetupHashPassword();

        var result = test.TestChangeCopilotUserInfo(new()
        {
            UserId = user.EntityId.ToString(),
            Email = HandlerTest.TestEmail,
            UserName = HandlerTest.TestUsername,
            Password = HandlerTest.TestPassword,
            Role = UserRole.Uploader.ToString(),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserActivated.Should().BeTrue();
        user.Email.Should().Be(HandlerTest.TestEmail);
        user.UserName.Should().Be(HandlerTest.TestUsername);
        user.Password.Should().Be(HandlerTest.TestHashedPassword);
        user.UserRole.Should().Be(UserRole.Uploader);
    }
}
