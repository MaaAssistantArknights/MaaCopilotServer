// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Test.TestEntities;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.UpdateCopilotUserPassword;

/// <summary>
/// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class UpdateCopilotUserPasswordCommandTest
{
    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/> with wrong original password.
    /// </summary>
    [TestMethod]
    public void TestHandleOriginalPasswordWrong()
    {
        var user = new CopilotUserFactory { Password = HandlerTest.TestHashedPassword }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);
        test.SecretService.SetupVerifyPassword(HandlerTest.TestHashedPassword, "wrong_password", false);

        var result = test.TestUpdateCopilotUserPassword(new()
        {
            OriginalPassword = "wrong_password",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/> with wrong original password.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new CopilotUserFactory { Password = HandlerTest.TestHashedPassword }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUser(user);
        test.SecretService.SetupVerifyPassword(HandlerTest.TestHashedPassword, HandlerTest.TestPassword, true);
        test.SecretService.SetupHashPassword("new_password", "new_password_hash");

        var result = test.TestUpdateCopilotUserPassword(new()
        {
            OriginalPassword = HandlerTest.TestPassword,
            NewPassword = "new_password",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Password.Should().Be("new_password_hash");
    }
}
