// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Infrastructure.Services;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;

namespace MaaCopilotServer.Application.Test.CopilotOperation.Commands.DeleteCopilotOperation;

/// <summary>
/// Tests for <see cref="DeleteCopilotOperationCommandHandler"/>.
/// </summary>
[TestClass]
public class DeleteCopilotOperationCommandTest
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly Resources.ApiErrorMessage _apiErrorMessage = new();

    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotIdService _copilotIdService = new CopilotIdService();

    /// <summary>
    ///     The service for current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService = Mock.Of<ICurrentUserService>(
        x => x.GetUserIdentity() == Guid.Empty);

    /// <summary>
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext = new TestDbContext();

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with the same user.
    /// </summary>
    [TestMethod]
    public void TestHandle_SameUser()
    {
        var user = new Domain.Entities.CopilotUser(
            string.Empty,
            string.Empty,
            string.Empty,
            UserRole.User,
            Guid.Empty);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var entity = new Domain.Entities.CopilotOperation(
            1,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            user,
            Guid.Empty,
            new List<string>(),
            new List<string>());
        _dbContext.CopilotOperations.Add(entity);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand()
        {
            Id = _copilotIdService.EncodeId(entity.Id)
        }, new CancellationToken()).GetAwaiter().GetResult();

        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        _dbContext.CopilotOperations.Any().Should().BeFalse();
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
        var user = new Domain.Entities.CopilotUser(
            string.Empty,
            string.Empty,
            string.Empty,
            requesterRole,
            Guid.Empty);
        var author = new Domain.Entities.CopilotUser(
            string.Empty,
            string.Empty,
            string.Empty,
            authorRole,
            Guid.Empty);
        _dbContext.CopilotUsers.Add(user);
        _dbContext.CopilotUsers.Add(author);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var entity = new Domain.Entities.CopilotOperation(
            1,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            author,
            Guid.Empty,
            new List<string>(),
            new List<string>());
        _dbContext.CopilotOperations.Add(entity);
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(user);

        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand()
        {
            Id = _copilotIdService.EncodeId(entity.Id)
        }, new CancellationToken()).GetAwaiter().GetResult();

        if (expectedToSucceed)
        {
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            _dbContext.CopilotOperations.Any().Should().BeFalse();
        }
        else
        {
            result.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            _dbContext.CopilotOperations.Any().Should().BeTrue();
        }
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with non-existing ID.
    /// </summary>
    [TestMethod]
    public void TestHandle_OperationNotFound()
    {
        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.GetUser().Result).Returns(new Domain.Entities.CopilotUser(string.Empty, string.Empty, string.Empty, UserRole.User, null));

        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, currentUserService.Object, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand()
        {
            Id = _copilotIdService.EncodeId(1)
        }, new CancellationToken()).GetAwaiter().GetResult();

        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
