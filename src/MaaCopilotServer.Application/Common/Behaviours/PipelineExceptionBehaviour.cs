// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

public class PipelineExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    // ReSharper disable once ContextualLoggerProblem
    public PipelineExceptionBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
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
                (string)LoggingType.FailedRequest, ex.Result.RealStatusCode, requestName, request);

            throw;
        }
    }
}
