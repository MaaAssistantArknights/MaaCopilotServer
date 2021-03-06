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
/// <typeparam name="TResponse">The type of the response, it will always be <see cref="MaaApiResponse"/>.</typeparam>
/// <remarks>You can not remove the unused <typeparamref name="TResponse"/>, or the service can not be injected to DI container.</remarks>
// ReSharper disable once UnusedTypeParameter
public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, MaaApiResponse>
    where TRequest : IRequest<MaaApiResponse> where TResponse : MaaApiResponse
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
    ///     The constructor.
    /// </summary>
    /// <param name="currentUserService">The service of current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public AuthorizationBehaviour(
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
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
    public async Task<MaaApiResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<MaaApiResponse> next)
    {
        var authorizeAttribute = request.GetType().ReadAttribute<AuthorizedAttribute>();
        if (authorizeAttribute is null)
        {
            return await next();
        }

        var userId = _currentUserService.GetUserIdentity();
        if (userId is null)
        {
            return MaaApiResponseHelper.Unauthorized(_apiErrorMessage.Unauthorized);
        }

        var user = await _currentUserService.GetUser();
        if (user is null)
        {
            return MaaApiResponseHelper.NotFound(string.Format(_apiErrorMessage.UserWithIdNotFound!, userId));
        }

        if (user.UserRole < authorizeAttribute.Role)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied);
        }

        if (authorizeAttribute.AllowInActivated is false && user.UserActivated is false)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.UserInactivated);
        }

        return await next();
    }
}
