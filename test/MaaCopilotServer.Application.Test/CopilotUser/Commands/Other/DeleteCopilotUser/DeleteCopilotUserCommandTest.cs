// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Other.DeleteCopilotUser;

/// <summary>
/// Tests <see cref="DeleteCopilotUserCommandHandler"/>
/// </summary>
[TestClass]
public class DeleteCopilotUserCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="DeleteCopilotUserCommandHandler.Handle(DeleteCopilotUserCommand, CancellationToken)"/>
    /// with user not found.
    /// </summary>
    [TestMethod]
    public void TestHandle_UserNotFound()
    {
        var result = new HandlerTest()
            .TestDeleteCopilotUser(new()
            {
                UserId = Guid.Empty.ToString(),
            });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotUserCommandHandler.Handle(DeleteCopilotUserCommand, CancellationToken)"/>
    /// with insufficient permission.
    /// </summary>
    [TestMethod]
    public void TestHandle_InsufficientPermission()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.Admin, null);

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.AddRange(user, @operator))
            .SetupGetUser(@operator)
            .TestDeleteCopilotUser(new()
            {
                UserId = user.EntityId.ToString(),
            });

        result.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotUserCommandHandler.Handle(DeleteCopilotUserCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.User, null);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.Admin, null);

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.AddRange(user, @operator))
            .SetupGetUser(@operator)
            .TestDeleteCopilotUser(new()
            {
                UserId = user.EntityId.ToString(),
            });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.DbContext.CopilotUsers.Count().Should().Be(1);
        result.DbContext.CopilotUsers.FirstOrDefault()!.EntityId.Should().Be(@operator.EntityId);
    }
}
