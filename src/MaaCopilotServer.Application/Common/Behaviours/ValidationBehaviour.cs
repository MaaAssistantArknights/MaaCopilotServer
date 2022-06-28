// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
///     The behaviour to validate request inputs.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response, it will always be <see cref="MaaApiResponse"/>.</typeparam>
/// <remarks>You can not remove the unused <typeparamref name="TResponse"/>, or the service can not be injected to DI container.</remarks>
// ReSharper disable once UnusedTypeParameter
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, MaaApiResponse>
    where TRequest : IRequest<MaaApiResponse> where TResponse : MaaApiResponse
{
    /// <summary>
    ///     The service of current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The validators.
    /// </summary>
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    ///     The constructor.
    /// </summary>
    /// <param name="validators">The validators.</param>
    /// <param name="currentUserService">The service of current user.</param>
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, ICurrentUserService currentUserService)
    {
        _validators = validators;
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
        if (_validators.Any() is false)
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        var failureMessage = string.Join("\n", failures.Select(f => f.ToString()));
        if (failures.Any())
        {
            return MaaApiResponseHelper.BadRequest(failureMessage);
        }

        return await next();
    }
}
