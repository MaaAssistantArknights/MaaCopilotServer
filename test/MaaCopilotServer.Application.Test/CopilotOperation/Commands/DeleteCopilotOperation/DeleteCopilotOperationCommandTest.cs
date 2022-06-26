// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Test.TestHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
    private Resources.ApiErrorMessage _apiErrorMessage;

    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private ICopilotIdService _copilotIdService;

    /// <summary>
    ///     The service for current user.
    /// </summary>
    private ICurrentUserService _currentUserService;

    /// <summary>
    ///     The DB context.
    /// </summary>
    private IMaaCopilotDbContext _dbContext;

    /// <summary>
    /// Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _copilotIdService = Substitute.For<ICopilotIdService>();

        _currentUserService = Substitute.For<ICurrentUserService>();
        _currentUserService.GetUserIdentity().Returns(Guid.Empty);

        _dbContext = new TestDbContext();

        _apiErrorMessage = new Resources.ApiErrorMessage();
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>.
    /// </summary>
    [TestMethod]
    public void TestHandle()
    {
        _dbContext.CopilotOperations.Add(new Domain.Entities.CopilotOperation(
            10000,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            new Domain.Entities.CopilotUser(
                string.Empty,
                string.Empty,
                string.Empty,
                Domain.Enums.UserRole.User,
                Guid.Empty),
            Guid.Empty,
            new List<string>()));
        _dbContext.SaveChangesAsync(new CancellationToken()).Wait();

        _copilotIdService.DecodeId(Arg.Any<string>()).ReturnsForAnyArgs(10000);
        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, _currentUserService, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand(), new CancellationToken())
                            .GetAwaiter()
                            .GetResult();

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
        _copilotIdService.DecodeId(Arg.Any<string>()).ReturnsForAnyArgs((long?)null);

        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, _currentUserService, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand(), new CancellationToken())
                            .GetAwaiter()
                            .GetResult();

        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    /// <summary>
    /// Tests <see cref="DeleteCopilotOperationCommandHandler.Handle(DeleteCopilotOperationCommand, CancellationToken)"/>
    /// with non-existing ID.
    /// </summary>
    [TestMethod]
    public void TestHandle_IdNotFound()
    {
        _copilotIdService.DecodeId(Arg.Any<string>()).ReturnsForAnyArgs(10000);

        var handler = new DeleteCopilotOperationCommandHandler(
            _dbContext, _copilotIdService, _currentUserService, _apiErrorMessage);
        var result = handler.Handle(new DeleteCopilotOperationCommand(), new CancellationToken())
                            .GetAwaiter()
                            .GetResult();

        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
}
