// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
///     The behaviour to convert unhandled exceptions to <see cref="PipelineException" />.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage;

    /// <summary>
    ///     The service of current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The logger.
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    ///     The constructor of <see cref="UnhandledExceptionBehaviour{TRequest, TResponse}" />.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="currentUserService">The service of current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    // ReSharper disable once ContextualLoggerProblem
    public UnhandledExceptionBehaviour(
        ILogger<TRequest> logger,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    ///     The handler of the request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="next">The next request handler.</param>
    /// <returns>The response.</returns>
    /// <exception cref="PipelineException">Thrown when there are exceptions thrown.</exception>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        try
        {
            return await next();
        }
        catch (PipelineException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex,
                "MaaCopilotServer: Type -> {LoggingType}; Request Name -> {Name}; Request -> {@Request};",
                LoggingType.Exception, requestName, request);

            throw new PipelineException(MaaApiResponseHelper.InternalError(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException));
        }
    }
}
