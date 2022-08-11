// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Domain.Enums;
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
        var user = new CopilotUserFactory().Build();
        var entity = new CopilotOperationFactory { Author = user }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotOperations.Add(entity);
        });
        test.CurrentUserService.SetupGetUser(user);
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
        var user = new CopilotUserFactory { UserRole = requesterRole }.Build();
        var author = new CopilotUserFactory { UserRole = authorRole }.Build();
        var entity = new CopilotOperationFactory { Author = author }.Build();

        var test = new HandlerTest();
        test.DbContext.Setup(db =>
        {
            db.CopilotUsers.Add(user);
            db.CopilotUsers.Add(author);
            db.CopilotOperations.Add(entity);
        });
        test.CurrentUserService.SetupGetUser(user);
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
        test.CurrentUserService.SetupGetUser(new CopilotUserFactory().Build());

        var result = test.TestDeleteCopilotOperation(new()
        {
            Id = EntityIdHelper.EncodeId(1),
        });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
