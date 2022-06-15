// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
/// The behaviour to record the performance data, e.g. time elapsed of a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// The service of current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    /// A timer to calculate the time elapsed.
    /// </summary>
    private readonly Stopwatch _timer;

    /// <summary>
    /// The constructor of <see cref="PerformanceBehaviour{TRequest, TResponse}"/>.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="currentUserService">The service of current user.</param>
    public PerformanceBehaviour(
        // ReSharper disable once ContextualLoggerProblem
        ILogger<TRequest> logger,
        ICurrentUserService currentUserService)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// The handler of the request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="next">The next request handler.</param>
    /// <returns>The response.</returns>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        _timer.Start();

        try
        {
            return await next();
        }
        finally
        {
            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            var requestName = typeof(TRequest).Name;
            var userId = _currentUserService.GetUserIdentity().ToString() ?? string.Empty;

            _logger.LogInformation(
                "MaaCopilotServer: Type -> {LoggingType}; Request Name -> {Name}; Time -> {ElapsedTime}; User -> {UserId}; Request -> {@Request}",
                (string)LoggingType.Request, requestName, elapsedMilliseconds, userId, request);
        }
    }
}
