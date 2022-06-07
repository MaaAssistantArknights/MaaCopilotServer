// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Security;
using MaaCopilotServer.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

[Authorized(UserRole.Admin)]
public record DeleteCopilotUserCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("user_id")] public string? UserId { get; set; }
}

public class DeleteCopilotUserCommandHandler : IRequestHandler<DeleteCopilotUserCommand, MaaActionResult<EmptyObject>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCopilotUserCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<EmptyObject>> Handle(DeleteCopilotUserCommand request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);
        if (user == null)
        {
            return MaaApiResponse.NotFound($"User \"{userId}\"", _currentUserService.GetTrackingId());
        }

        var @operator = await _dbContext.CopilotUsers.FirstOrDefaultAsync(
            x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);
        if (@operator is null)
        {
            throw new PipelineException(MaaApiResponse.InternalError(_currentUserService.GetTrackingId()));
        }

        if (@operator.UserRole <= user.UserRole)
        {
            return MaaApiResponse.Forbidden(_currentUserService.GetTrackingId(), "You cannot delete a user with a higher or equal role than you.");
        }

        user.Delete(_currentUserService.GetUserIdentity()!.Value);
        _dbContext.CopilotUsers.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
