// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TRequest> _logger;
    private readonly Stopwatch _timer;

    public PerformanceBehaviour(
        // ReSharper disable once ContextualLoggerProblem
        ILogger<TRequest> logger,
        ICurrentUserService currentUserService)
    {
        _timer = new Stopwatch();

        _logger = logger;
        _currentUserService = currentUserService;
    }

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
