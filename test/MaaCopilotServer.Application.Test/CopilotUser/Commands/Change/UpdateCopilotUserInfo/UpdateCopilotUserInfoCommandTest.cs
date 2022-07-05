// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Application.Test.TestHelpers;
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
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .TestUpdateCopilotUserInfo(new()
            {
                UserName = "new_username",
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.UserName.Should().Be("new_username");
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes,
    /// but email is already in use.
    /// </summary>
    [TestMethod]
    public void TestHandleEmailAlreadyInUse()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var user2 = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotUsers.Add(user2))
            .SetupGetUser(user)
            .TestUpdateCopilotUserInfo(new()
            {
                Email = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes,
    /// but the activation email failed to send.
    /// </summary>
    [TestMethod]
    public void TestHandleActivationEmailFailedToSend()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .SetupGenerateToken()
            .SetupSendEmailAsync(false)
            .TestUpdateCopilotUserInfo(new()
            {
                Email = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserInfoCommandHandler.Handle(UpdateCopilotUserInfoCommand, CancellationToken)"/> with email changes.
    /// </summary>
    [TestMethod]
    public void TestHandleChangeEmail()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        user.ActivateUser(Guid.Empty);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .SetupGenerateToken()
            .SetupSendEmailAsync(true)
            .TestUpdateCopilotUserInfo(new()
            {
                Email = HandlerTest.TestEmail,
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Email.Should().Be(HandlerTest.TestEmail);
        user.UserActivated.Should().BeFalse();
    }
}
