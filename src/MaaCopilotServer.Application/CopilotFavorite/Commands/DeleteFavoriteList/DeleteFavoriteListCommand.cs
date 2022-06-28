// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.DeleteFavoriteList;

/// <summary>
///     The DTO for the DeleteFavoriteList command.
/// </summary>
[Authorized(UserRole.User)]
public record DeleteFavoriteListCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The ID of the favorite list to delete.
    /// </summary>
    [Required]
    [JsonPropertyName("favorite_list_id")]
    public string? FavoriteListId { get; set; }
}

public class DeleteFavoriteListCommandHandler : IRequestHandler<DeleteFavoriteListCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;

    public DeleteFavoriteListCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(DeleteFavoriteListCommand request, CancellationToken cancellationToken)
    {
        var listId = Guid.Parse(request.FavoriteListId!);
        var userId = _currentUserService.GetUserIdentity()!.Value;
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);
        if (user is null)
        {
            return MaaApiResponseHelper.InternalError();
        }

        var list = await _dbContext.CopilotUserFavorites
            .Include(x => x.User)
            .Include(x => x.Operations)
            .FirstOrDefaultAsync(x => x.EntityId == listId, cancellationToken);
        if (list is null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.FavListWithIdNotFound!, listId));
        }

        if (user.IsAllowAccess(list.User) is false)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied);
        }

        list.Operations.ForEach(x => x.RemoveFavorites(userId));
        _dbContext.CopilotUserFavorites.Remove(list);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
