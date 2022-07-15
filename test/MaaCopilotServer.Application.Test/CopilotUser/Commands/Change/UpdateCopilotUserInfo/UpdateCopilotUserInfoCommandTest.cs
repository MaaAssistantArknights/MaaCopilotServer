// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.UpdateCopilotUserInfo;

/// <summary>
/// Tests <see cref="UpdateCopilotUserInfoCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class UpdateCopilotUserInfoCommandTest
{
    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with username changes.
    /// </summary>
    [TestMethod]
    public void TestHandleChangeUsername()
    {
        var user = new CopilotUserFactory().Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);

        var result = test.TestUpdateCopilotUserInfo(new()
        {
            UserName = "new_username",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserName.Should().Be("new_username");
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes,
    /// but email is already in use.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailAlreadyInUse()
    {
        var user = new CopilotUserFactory().Build();
        var user2 = new CopilotUserFactory().Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.DbContext.Setup(db => db.CopilotUsers.Add(user2));
        test.CurrentUserService.SetupGetUser(user);

        var result = test.TestUpdateCopilotUserInfo(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes,
    /// but the activation email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandleActivationEmailFailedToSend()
    {
        var user = new CopilotUserFactory() { Email = string.Empty }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(false);

        var result = test.TestUpdateCopilotUserInfo(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes.
    /// </summary>
    [TestMethod]
    public void TestHandleChangeEmail()
    {
        var user = new CopilotUserFactory() { Email = string.Empty }.Build();
        user.ActivateUser(Guid.Empty);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);
        test.SecretService.SetupGenerateToken();
        test.MailService.SetupSendEmailAsync(true);

        var result = test.TestUpdateCopilotUserInfo(new()
        {
            Email = HandlerTest.TestEmail,
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Email.Should().Be(HandlerTest.TestEmail);
        user.UserActivated.Should().BeFalse();
    }
}
