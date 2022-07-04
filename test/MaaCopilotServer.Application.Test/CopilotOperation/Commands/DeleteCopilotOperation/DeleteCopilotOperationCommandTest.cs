// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.Test.TestHelpers;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.DeleteCopilotOperation;

/// <summary>
/// Tests <see cref="DeleteCopilotOperationCommandHandler"/>.
/// </summary>
[TestClass]
public class DeleteCopilotOperationCommandTest
{
    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotIdService _copilotIdService = new CopilotIdService();

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with the same user.
    /// </summary>
    [TestMethod]
    public void TestHandle_SameUser()
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, Guid.Empty);
        var entity = new Domain.Entities.CopilotOperation(1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, user, Guid.Empty, new List<string>(), new List<string>());

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotOperations.Add(entity))
            .SetupGetUser(user).TestDeleteCopilotOperation(new()
            {
                Id = _copilotIdService.EncodeId(entity.Id)
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
    public void TestHandle_DifferentUsers(UserRole requesterRole, UserRole authorRole, bool expectedToSucceed)
    {
        var user = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, requesterRole, Guid.Empty);
        var author = new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, authorRole, Guid.Empty);
        var entity = new Domain.Entities.CopilotOperation(1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, author, Guid.Empty, new List<string>(), new List<string>());

        var result = new HandlerTest()
            .SetupDatabase(db => db.CopilotUsers.Add(user))
            .SetupDatabase(db => db.CopilotUsers.Add(author))
            .SetupDatabase(db => db.CopilotOperations.Add(entity))
            .SetupGetUser(user).TestDeleteCopilotOperation(new()
            {
                Id = _copilotIdService.EncodeId(entity.Id)
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
    public void TestHandle_OperationNotFound()
    {
        var result = new HandlerTest()
            .SetupGetUser(new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, null))
            .TestDeleteCopilotOperation(new()
            {
                Id = _copilotIdService.EncodeId(1),
            });

        result.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
