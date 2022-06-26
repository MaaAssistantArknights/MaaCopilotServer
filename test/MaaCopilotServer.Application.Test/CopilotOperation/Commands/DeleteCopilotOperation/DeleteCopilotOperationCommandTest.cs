// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
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
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        var user = new Domain.Entities.CopilotUser(
            string.Empty,
            string.Empty,
            string.Empty,
            Domain.Enums.UserRole.User,
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
    /// with invalid ID.
    /// </summary>
    [TestMethod]
    public void TestHandle_InvalidId()
    {
        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, _currentUserService, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand()
        {
            Id = null
        }, new CancellationToken()).GetAwaiter().GetResult();

        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with non-existing ID.
    /// </summary>
    [TestMethod]
    public void TestHandle_IdNotFound()
    {
        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, _currentUserService, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand()
        {
            Id = _copilotIdService.EncodeId(1)
        }, new CancellationToken()).GetAwaiter().GetResult();

        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
