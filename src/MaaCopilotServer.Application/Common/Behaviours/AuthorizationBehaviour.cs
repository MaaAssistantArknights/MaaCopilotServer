// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Security;
using MediatR;

namespace MaaCopilotServer.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;

    public AuthorizationBehaviour(IIdentityService identityService, ICurrentUserService currentUserService)
    {
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizedAttribute>().ToList();
        if (authorizeAttributes.Count == 0)
        {
            return await next();
        }

        var userId = _currentUserService.GetUserIdentity();
        if (userId is null)
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _identityService.GetUserAsync(userId.Value);
        if (user is null)
        {
            throw new UserNotFoundException(userId.ToString()!);
        }

        var roleRequired = authorizeAttributes.First().Role;
        if (user.UserRole < roleRequired)
        {
            throw new UserAccessDeniedException(user.UserRole, roleRequired);
        }

        return await next();
    }
}
