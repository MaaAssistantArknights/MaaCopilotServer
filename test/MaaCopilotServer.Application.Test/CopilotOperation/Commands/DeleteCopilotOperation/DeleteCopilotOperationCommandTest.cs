// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.Test.TestExtensions;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.GameData.Entity;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.DeleteCopilotOperation;

/// <summary>
/// Tests <see cref="DeleteCopilotOperationCommandHandler"/>.
/// </summary>
[TestClass]
[ExcludeFromCodeCoverage]
public class DeleteCopilotOperationCommandTest
{
    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with the same user.
    /// </summary>
    [TestMethod]
    public void TestHandleSameUser()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, Guid.Empty);
        var entity = new Domain.Entities.CopilotOperation(0, string.Empty, string.Empty,
            string.Empty, string.Empty, user, Guid.Empty,
            new ArkLevelData(new ArkLevelEntityGlobal()),
            new List<string>(), new List<string>());

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotOperations.Add(entity);
        });
        test.CurrentUserService.SetupGetUser(user);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var result = test.TestDeleteCopilotOperation(new()
        {
            Id = EntityIdHelper.EncodeId(entity.Id),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.DbContext.CopilotOperations.Any().Should().BeFalse();
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with accessing from an account to content created by other people.
    /// </summary>
    [DataTestMethod]
    [DataRow(UserRole.User, UserRole.User, false)]
    [DataRow(UserRole.Admin, UserRole.User, true)]
    [DataRow(UserRole.Admin, UserRole.Uploader, true)]
    [DataRow(UserRole.Admin, UserRole.Admin, false)]
    [DataRow(UserRole.Admin, UserRole.SuperAdmin, false)]
    [DataRow(UserRole.SuperAdmin, UserRole.User, true)]
    [DataRow(UserRole.SuperAdmin, UserRole.Uploader, true)]
    [DataRow(UserRole.SuperAdmin, UserRole.Admin, true)]
    [DataRow(UserRole.SuperAdmin, UserRole.SuperAdmin, true)]
    public void TestHandleDifferentUsers(UserRole requesterRole, UserRole authorRole, bool expectedToSucceed)
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, requesterRole, Guid.Empty);
        var author = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, authorRole, Guid.Empty);
        var entity = new Domain.Entities.CopilotOperation(0, string.Empty, string.Empty,
            string.Empty, string.Empty, author, Guid.Empty,
            new ArkLevelData(new ArkLevelEntityGlobal()),
            new List<string>(), new List<string>());

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotUsers.Add(author);
            db.CopilotOperations.Add(entity);
        });
        test.CurrentUserService.SetupGetUser(user);
        test.CopilotOperationService.SetupDecodeAndEncodeId();
        var result = test.TestDeleteCopilotOperation(new()
        {
            Id = EntityIdHelper.EncodeId(entity.Id),
        });

        if (expectedToSucceed)
        {
            result.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.DbContext.CopilotOperations.Any().Should().BeFalse();
        }
        else
        {
            result.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            result.DbContext.CopilotOperations.Any().Should().BeTrue();
        }
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with non-existing ID.
    /// </summary>
    [TestMethod]
    public void TestHandleOperationNotFound()
    {
        var test = new HandlerTest();
        test.CurrentUserService.SetupGetUser(new(string.Empty, string.Empty, string.Empty, UserRole.User, null));
        test.CopilotOperationService.SetupDecodeAndEncodeId();

        var result = test.TestDeleteCopilotOperation(new()
        {
            Id = EntityIdHelper.EncodeId(1),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
