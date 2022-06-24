// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Extensions;

namespace MaaCopilotServer.Application.Common.Behaviours;

/// <summary>
///     The behaviour to check user identity and roles.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, MaaApiResponse<TResponse>>
    where TRequest : IRequest<MaaApiResponse<TResponse>>
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
    ///     The service of identity.
    /// </summary>
    private readonly IIdentityService _identityService;

    /// <summary>
    ///     The constructor of <see cref="AuthorizationBehaviour{TRequest, TResponse}" />.
    /// </summary>
    /// <param name="identityService">The service of identity.</param>
    /// <param name="currentUserService">The service of current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public AuthorizationBehaviour(
        IIdentityService identityService,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _identityService = identityService;
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
    /// <exception cref="PipelineException">
    ///     Thrown when the user ID/user is invalid, or the user role is insufficient.
    /// </exception>
    public async Task<MaaApiResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<MaaApiResponse<TResponse>> next)
    {
        var authorizeAttribute = request.GetType().ReadAttribute<AuthorizedAttribute>();
        if (authorizeAttribute is null)
        {
            return await next();
        }

        var userId = _currentUserService.GetUserIdentity();
        if (userId is null)
        {
            return MaaApiResponseHelper.Unauthorized<TResponse>(_currentUserService.GetTrackingId(),
                _apiErrorMessage.Unauthorized);
        }

        var user = await _identityService.GetUserAsync(userId.Value);
        if (user is null)
        {
            return MaaApiResponseHelper.NotFound<TResponse>(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, userId));
        }

        if (user.UserRole < authorizeAttribute.Role)
        {
            return MaaApiResponseHelper.Forbidden<TResponse>(_currentUserService.GetTrackingId(),
                _apiErrorMessage.PermissionDenied);
        }

        if (authorizeAttribute.AllowInActivated is false && user.UserActivated is false)
        {
            return MaaApiResponseHelper.Forbidden<TResponse>(_currentUserService.GetTrackingId(),
                _apiErrorMessage.UserInactivated);
        }

        return await next();
    }
}
