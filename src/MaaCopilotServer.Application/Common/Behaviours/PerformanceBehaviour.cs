// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
///     The behaviour to record the performance data, e.g. time elapsed of a request.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response, it will always be <see cref="MaaApiResponse"/>.</typeparam>
/// <remarks>You can not remove the unused <see cref="TResponse"/>, or the service can not be injected to DI container.</remarks>
// ReSharper disable once UnusedTypeParameter
public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, MaaApiResponse>
    where TRequest : IRequest<MaaApiResponse> where TResponse : MaaApiResponse
{
    /// <summary>
    ///     The service of current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The logger.
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    ///     A timer to calculate the time elapsed.
    /// </summary>
    private readonly Stopwatch _timer;

    /// <summary>
    ///     The constructor.
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
    ///     The handler of the request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="next">The next request handler.</param>
    /// <returns>The response.</returns>
    public async Task<MaaApiResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<MaaApiResponse> next)
    {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.GetUserIdentity().ToString() ?? string.Empty;
        var statusCode = response.StatusCode;

        var level = statusCode switch
        {
            >= 200 and < 400 => LogLevel.Information,
            >= 500 and < 600 => LogLevel.Error,
            _ => LogLevel.Warning
        };

        _logger.Log(level,
            "MaaCopilotServer: Type -> {LoggingType}; Name -> {Name}; Time -> {ElapsedTime}; Status -> {StatusCode}, User -> {UserId}; Request -> {@Request}",
            LoggingType.Request, requestName, elapsedMilliseconds.ToString(), statusCode.ToString(), userId, request);
        return response;
    }
}
