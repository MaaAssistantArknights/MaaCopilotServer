// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Test.Common.Behaviours;

/// <summary>
///     Tests of <see cref="PerformanceBehaviour{TRequest,TResponse}" />.
/// </summary>
[TestClass]
public class PerformanceBehaviourTest
{
    /// <summary>
    ///     The service of current user.
    /// </summary>
    private ICurrentUserService _currentUserService;

    /// <summary>
    ///     The logger.
    /// </summary>
    private ILogger<IRequest<string>> _logger;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _currentUserService = Substitute.For<ICurrentUserService>();
        _logger = Substitute.For<ILogger<IRequest<string>>>();
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="PerformanceBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, MediatR.RequestHandlerDelegate{TResponse})" />
    ///     .
    /// </summary>
    /// <returns>N/A</returns>
    [TestMethod]
    public async Task TestHandle()
    {
        var testUserId = new Guid();
        _currentUserService.GetUserIdentity().Returns(testUserId);
        var behaviour = new PerformanceBehaviour<IRequest<string>, string>(_logger, _currentUserService);
        var action = async () =>
            await behaviour.Handle(null, new CancellationToken(), () => Task.FromResult(string.Empty));
        await action.Should().NotThrowAsync();
    }
}
