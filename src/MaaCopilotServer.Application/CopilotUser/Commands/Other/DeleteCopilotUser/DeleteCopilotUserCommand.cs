// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

/// <summary>
///     The DTO for the DeleteCopilotUser command.
/// </summary>
[Authorized(UserRole.Admin)]
public record DeleteCopilotUserCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The id of the user that pending to delete.
    /// </summary>
    [Required]
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}

public class DeleteCopilotUserCommandHandler : IRequestHandler<DeleteCopilotUserCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
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

    public async Task<MaaApiResponse> Handle(DeleteCopilotUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);
        if (user == null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.UserWithIdNotFound!, userId));
        }

        var @operator = await _dbContext.CopilotUsers.FirstOrDefaultAsync(
            x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);
        if (@operator is null)
        {
            return MaaApiResponseHelper.InternalError(
                _apiErrorMessage.InternalException);
        }

        if (@operator.UserRole <= user.UserRole)
        {
            return MaaApiResponseHelper.Forbidden(
                _apiErrorMessage.PermissionDenied);
        }

        user.Delete(_currentUserService.GetUserIdentity()!.Value);
        _dbContext.CopilotUsers.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
