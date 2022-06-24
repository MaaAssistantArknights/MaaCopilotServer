// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
///     The behaviour to convert unhandled exceptions to 500 internal errors.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response, it will always be <see cref="MaaApiResponse"/>.</typeparam>
/// <remarks>You can not remove the unused <see cref="TResponse"/>, or the service can not be injected to DI container.</remarks>
// ReSharper disable once UnusedTypeParameter
public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, MaaApiResponse>
    where TRequest : IRequest<MaaApiResponse> where TResponse : MaaApiResponse
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage;


    /// <summary>
    ///     The logger.
    /// </summary>
    private readonly ILogger<TRequest> _logger;

    /// <summary>
    ///     The constructor.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    // ReSharper disable once ContextualLoggerProblem
    public UnhandledExceptionBehaviour(
        // ReSharper disable once ContextualLoggerProblem
        ILogger<TRequest> logger,
        ApiErrorMessage apiErrorMessage)
    {
        _logger = logger;
        _apiErrorMessage = apiErrorMessage;
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
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(ex,
                "MaaCopilotServer: Type -> {LoggingType}; Request Name -> {Name}; Request -> {@Request};",
                LoggingType.Exception, requestName, request);

            return MaaApiResponseHelper.InternalError(_apiErrorMessage.InternalException);
        }
    }
}
