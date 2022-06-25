// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Behaviours;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    private ILogger<IRequest<MaaApiResponse>> _logger;

    /// <summary>
    ///     Initializes tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
        _currentUserService = Substitute.For<ICurrentUserService>();
        _logger = new TestLogger();
    }

    /// <summary>
    ///     Tests
    ///     <see
    ///         cref="PerformanceBehaviour{TRequest, TResponse}.Handle(TRequest, CancellationToken, RequestHandlerDelegate{TResponse})" />
    ///     .
    /// </summary>
    /// <returns>N/A</returns>
    [DataTestMethod]
    [DataRow(StatusCodes.Status200OK, LogLevel.Information)]
    [DataRow(StatusCodes.Status400BadRequest, LogLevel.Warning)]
    [DataRow(StatusCodes.Status401Unauthorized, LogLevel.Warning)]
    [DataRow(StatusCodes.Status403Forbidden, LogLevel.Warning)]
    [DataRow(StatusCodes.Status404NotFound, LogLevel.Warning)]
    [DataRow(StatusCodes.Status500InternalServerError, LogLevel.Error)]
    public async Task TestHandle(int statusCode, LogLevel expectedLogLevel)
    {
        var testUserId = new Guid();
        _currentUserService.GetUserIdentity().Returns(testUserId);
        var behaviour =
            new PerformanceBehaviour<IRequest<MaaApiResponse>, MaaApiResponse>(_logger, _currentUserService);
        await behaviour.Handle(null, new CancellationToken(), () => Task.FromResult(new MaaApiResponse()
        {
            StatusCode = statusCode,
        }));
        var testLogger = (TestLogger)_logger;
        testLogger.Called.Should()
                         .HaveCount(1)
                         .And
                         .Contain(expectedLogLevel);
    }

    /// <summary>
    /// The test class for logger.
    /// </summary>
    private class TestLogger : ILogger<IRequest<MaaApiResponse>>
    {
        /// <summary>
        /// Called arguments of log level.
        /// </summary>
        public readonly List<LogLevel> Called = new();

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state) => default;

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel,
                                EventId eventId,
                                TState state,
                                Exception? exception,
                                Func<TState, Exception?, string> formatter)
        {
            Called.Add(logLevel);
        }
    }
}
