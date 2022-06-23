// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
///     The behaviour to validate request inputs.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
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
    ///     The constructor of <see cref="ValidationBehaviour{TRequest, TResponse}" />.
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
    /// <exception cref="PipelineException">Thrown when there are validation errors.</exception>
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
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
            throw new PipelineException(MaaActionResultHelper.BadRequest(_currentUserService.GetTrackingId(), failureMessage));
        }

        return await next();
    }
}
