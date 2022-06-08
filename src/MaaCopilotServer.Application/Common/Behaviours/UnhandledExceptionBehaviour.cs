// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;

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

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
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
            _logger.LogError(exception: ex,
                "MaaCopilotServer: Type -> {LoggingType}; Request Name -> {Name}; Request -> {@Request};",
                (string)LoggingType.Exception, requestName, request);

            throw new PipelineException(MaaApiResponse.InternalError(_currentUserService.GetTrackingId(), _apiErrorMessage.InternalException));
        }
    }
}
