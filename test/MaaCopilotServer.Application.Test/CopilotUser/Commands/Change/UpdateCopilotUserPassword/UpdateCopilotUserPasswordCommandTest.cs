// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Change.UpdateCopilotUserPassword;

/// <summary>
/// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/>.
/// </summary>
[TestClass]
public class UpdateCopilotUserPasswordCommandTest
{
    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/> with wrong original password.
    /// </summary>
    [TestMethod]
    public void TestHandle_OriginalPasswordWrong()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, HandlerTest.TestHashedPassword, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .SetupVerifyPassword(HandlerTest.TestHashedPassword, "wrong_password", false)
            .TestUpdateCopilotUserPassword(new()
            {
                OriginalPassword = "wrong_password",
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="UpdateCopilotUserPasswordCommandHandler"/> with wrong original password.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, HandlerTest.TestHashedPassword, string.Empty, Domain.Enums.UserRole.User, null);
        var response = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupGetUser(user)
            .SetupVerifyPassword(HandlerTest.TestHashedPassword, HandlerTest.TestPassword, true)
            .SetupHashPassword("new_password", "new_password_hash")
            .TestUpdateCopilotUserPassword(new()
            {
                OriginalPassword = HandlerTest.TestPassword,
                NewPassword = "new_password",
            })
            .Response;

        response.StatusCode.Should().Be(StatusCodes.Status200OK);
        user.Password.Should().Be("new_password_hash");
    }
}
