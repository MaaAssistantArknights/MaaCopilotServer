// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
///     The behaviour to handle pipeline exceptions.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class PipelineExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, MaaApiResponse<TResponse>>
    where TRequest : IRequest<MaaApiResponse<TResponse>>
{
    /// <summary>
    ///     The logger.
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    ///     The constructor of <see cref="PipelineExceptionBehaviour{TRequest, TResponse}" />.
    /// </summary>
    /// <param name="logger">The logger.</param>
    // ReSharper disable once ContextualLoggerProblem
    public PipelineExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///     The handler of the request.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="next">The next request handler.</param>
    /// <returns>The response.</returns>
    public async Task<MaaApiResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<MaaApiResponse<TResponse>> next)
    {
        try
        {
            return await next();
        }
        catch (PipelineException ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(
                "MaaCopilotServer: Type -> {LoggingType}; Status Code -> {StatusCode}; Request Name -> {Name}; Request -> {@Request}",
                LoggingType.FailedRequest, ex.Result.StatusCode, requestName, request);

            throw;
        }
    }
}
