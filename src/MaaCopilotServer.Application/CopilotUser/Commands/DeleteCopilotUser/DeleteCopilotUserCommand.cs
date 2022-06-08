// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

[Authorized(UserRole.Admin)]
public record DeleteCopilotUserCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("user_id")] public string? UserId { get; set; }
}

public class DeleteCopilotUserCommandHandler : IRequestHandler<DeleteCopilotUserCommand, MaaActionResult<EmptyObject>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;

    public DeleteCopilotUserCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaActionResult<EmptyObject>> Handle(DeleteCopilotUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);
        if (user == null)
        {
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, userId)));
        }

        var @operator = await _dbContext.CopilotUsers.FirstOrDefaultAsync(
            x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);
        if (@operator is null)
        {
            throw new PipelineException(MaaApiResponse.InternalError(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException));
        }

        if (@operator.UserRole <= user.UserRole)
        {
            throw new PipelineException(MaaApiResponse.Forbidden(_currentUserService.GetTrackingId(),
                _apiErrorMessage.PermissionDenied));
        }

        user.Delete(_currentUserService.GetUserIdentity()!.Value);
        _dbContext.CopilotUsers.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
