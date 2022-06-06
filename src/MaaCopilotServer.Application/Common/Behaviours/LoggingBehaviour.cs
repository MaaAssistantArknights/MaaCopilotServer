// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace MaaCopilotServer.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly ICurrentUserService _currentUserService;

    // ReSharper disable once ContextualLoggerProblem
    public LoggingBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _currentUserService.GetUserIdentity()?.ToString() ?? "Anonymous";
        if (typeof(TRequest) == typeof(LoginCopilotUserCommand) || typeof(TRequest) == typeof(CreateCopilotUserCommand))
        {
            _logger.LogInformation("MaaCopilotServer Request: {Name} {UserId} {@Request}", requestName, userId, "********");
        }
        else
        {
            _logger.LogInformation("MaaCopilotServer Request: {Name} {UserId} {@Request}", requestName, userId, request);
        }
        return Task.CompletedTask;
    }
}
