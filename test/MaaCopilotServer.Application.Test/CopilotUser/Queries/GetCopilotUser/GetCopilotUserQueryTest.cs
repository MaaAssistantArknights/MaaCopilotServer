// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using MaaCopilotServer.Application.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotUser.Queries.GetCopilotUser;

/// <summary>
/// Tests <see cref="GetCopilotUserQueryHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class GetCopilotUserQueryHandlerTest
{
    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryHandler.Handle(GetCopilotUserQuery, CancellationToken)"/>
    /// with null current user identity.
    /// </summary>
    [TestMethod]
    public void TestHandleCurrentUserNull()
    {
        var test = new HandlerTest();
        test.CurrentUserService.SetupGetUserIdentity(null);

        var result = test.TestGetCopilotUser(new()
        {
            UserId = "me",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryHandler.Handle(GetCopilotUserQuery, CancellationToken)"/>
    /// with user not found.
    /// </summary>
    [TestMethod]
    public void TestHandleUserNotFound()
    {
        var test = new HandlerTest();
        test.CurrentUserService.SetupGetUserIdentity(null);

        var result = test.TestGetCopilotUser(new()
        {
            UserId = Guid.Empty.ToString(),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryHandler.Handle(GetCopilotUserQuery, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, HandlerTest.TestUsername, Domain.Enums.UserRole.User, null);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));

        var result = test.TestGetCopilotUser(new()
        {
            UserId = user.EntityId.ToString(),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var dto = ((GetCopilotUserDto)result.Response.Data!);
        dto.Id.Should().Be(user.EntityId);
        dto.UserName.Should().Be(HandlerTest.TestUsername);
        dto.UserRole.Should().Be(Domain.Enums.UserRole.User);
    }

    /// <summary>
    /// Tests <see cref="GetCopilotUserQueryHandler.Handle(GetCopilotUserQuery, CancellationToken)"/>
    /// with current user.
    /// </summary>
    [TestMethod]
    public void TestHandleCurrentUser()
    {
        var user = new Domain.Entities.CopilotUser(HandlerTest.TestEmail, string.Empty, HandlerTest.TestUsername, Domain.Enums.UserRole.User, null);

        var test = new HandlerTest();
        test.DbContext.Setup(db => db.CopilotUsers.Add(user));
        test.CurrentUserService.SetupGetUserIdentity(user.EntityId);

        var result = test.TestGetCopilotUser(new()
        {
            UserId = "me",
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        var dto = ((GetCopilotUserDto)result.Response.Data!);
        dto.Id.Should().Be(user.EntityId);
        dto.UserName.Should().Be(HandlerTest.TestUsername);
        dto.UserRole.Should().Be(Domain.Enums.UserRole.User);
    }
}
