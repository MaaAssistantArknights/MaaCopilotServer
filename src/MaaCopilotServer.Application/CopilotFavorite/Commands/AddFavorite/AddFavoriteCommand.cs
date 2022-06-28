// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.AddFavorite;

/// <summary>
///     The DTO for the AddFavorite command.
/// </summary>
[Authorized(UserRole.User)]
public record AddFavoriteCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The id of the favorite list.
    /// </summary>
    [Required]
    [JsonPropertyName("favorite_list_id")]
    public string? FavoriteListId { get; set; }

    /// <summary>
    ///     The operation id.
    /// </summary>
    [Required]
    [JsonPropertyName("operation_id")]
    public string? OperationId { get; set; }
}

public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ApiErrorMessage _apiErrorMessage;

    public AddFavoriteCommandHandler(
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

    public async Task<MaaApiResponse> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        // Get basic infos
        var listId = Guid.Parse(request.FavoriteListId!);
        var operationId = _copilotIdService.DecodeId(request.OperationId!);
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Get operation
        var operation = await _dbContext.CopilotOperations
            .FirstOrDefaultAsync(x => x.Id == operationId, cancellationToken);
        if (operation is null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.OperationId!));
        }

        // Get fav list
        var list = await _dbContext.CopilotUserFavorites
            .Include(x => x.User)
            .Include(x => x.Operations)
            .FirstOrDefaultAsync(x => x.EntityId == listId, cancellationToken);
        if (list is null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.FavListWithIdNotFound!, listId));
        }

        // Check if the user could access the resource
        if (user!.IsAllowAccess(list.User) is false)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied);
        }

        // Add fav item to list
        list.AddFavoriteOperation(operation, user.EntityId);
        operation.AddFavorites(user.EntityId);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
