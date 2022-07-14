// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Commands.Other.DeleteCopilotUser;

/// <summary>
/// Tests <see cref="DeleteCopilotUserCommandHandler"/>
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class DeleteCopilotUserCommandHandlerTest
{
    /// <summary>
    /// Tests <see cref="DeleteCopilotUserCommandHandler.Handle(DeleteCopilotUserCommand, CancellationToken)"/>
    /// with user not found.
    /// </summary>
    [TestMethod]
    public void TestHandleUserNotFound()
    {
        var test = new HandlerTest();

        var result = test.TestDeleteCopilotUser(new()
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
    public void TestHandleInsufficientPermission()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.SuperAdmin, null);
        var @operator = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, Domain.Enums.UserRole.Admin, null);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.AddRange(user, @operator));
        test.CurrentUserService.SetupGetUser(@operator);

        var result = test.TestDeleteCopilotUser(new()
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

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.AddRange(user, @operator));
        test.CurrentUserService.SetupGetUser(@operator);

        var result = test.TestDeleteCopilotUser(new()
        {
            UserId = user.EntityId.ToString(),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.DbContext.CopilotUsers.Count().Should().Be(1);
        result.DbContext.CopilotUsers.FirstOrDefault()!.EntityId.Should().Be(@operator.EntityId);
    }
}
