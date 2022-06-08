// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;

namespace MaaCopilotServer.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    public AuthorizationBehaviour(IIdentityService identityService, ICurrentUserService currentUserService)
    {
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizedAttribute>().ToList();
        if (authorizeAttributes.Count == 0)
        {
            return await next();
        }

        var userId = _currentUserService.GetUserIdentity();
        if (userId is null)
        {
            throw new PipelineException(MaaApiResponse.Unauthorized(_currentUserService.GetTrackingId()));
        }

        var user = await _identityService.GetUserAsync(userId.Value);
        if (user is null)
        {
            throw new PipelineException(MaaApiResponse.NotFound($"User {userId}", _currentUserService.GetTrackingId()));
        }

        var roleRequired = authorizeAttributes.First().Role;
        if (user.UserRole < roleRequired)
        {
            throw new PipelineException(MaaApiResponse.Forbidden(_currentUserService.GetTrackingId()));
        }

        return await next();
    }
}
