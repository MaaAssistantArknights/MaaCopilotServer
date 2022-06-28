// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.RemoveFavorite;

/// <summary>
///     The DTO for the RemoveFavorite command.
/// </summary>
[Authorized(UserRole.User)]
public record RemoveFavoriteCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The id of the favorite list.
    /// </summary>
    [Required]
    [JsonPropertyName("favorite_list_id")]
    public string? FavoriteListId { get; set; }

    /// <summary>
    ///     The id of the operation to remove.
    /// </summary>
    [JsonPropertyName("operation_id")]
    public string? OperationId { get; set; }
}

public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ApiErrorMessage _apiErrorMessage;

    public RemoveFavoriteCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ICopilotIdService copilotIdService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _copilotIdService = copilotIdService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
    {
        var listId = Guid.Parse(request.FavoriteListId!);
        var operationId = _copilotIdService.DecodeId(request.OperationId!);
        var user = await _currentUserService.GetUser();

        var operation = await _dbContext.CopilotOperations
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == operationId, cancellationToken);
        if (operation is null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.OperationId!));
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

        if (user!.IsAllowAccess(list.User) is false)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied);
        }

        list.RemoveFavoriteOperation(operation, user!.EntityId);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
