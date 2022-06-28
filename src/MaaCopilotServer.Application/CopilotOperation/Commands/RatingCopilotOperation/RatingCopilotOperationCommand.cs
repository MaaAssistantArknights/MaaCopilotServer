// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.RatingCopilotOperation;

/// <summary>
///     The DTO for the RatingCopilotOperation command.
/// </summary>
[Authorized(UserRole.User)]
public record RatingCopilotOperationCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The id of the operation.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    ///     The rating type of the operation. Could be Like, Dislike or None.
    /// </summary>
    [Required]
    [JsonPropertyName("rating")]
    public string? RatingType { get; set; }
}

public class RatingCopilotOperationCommandHandler : IRequestHandler<RatingCopilotOperationCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ApiErrorMessage _apiErrorMessage;

    public RatingCopilotOperationCommandHandler(
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

    public async Task<MaaApiResponse> Handle(RatingCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        // Get current infos
        var user = (await _currentUserService.GetUser()).IsNotNull();
        var ratingType = Enum.Parse<OperationRatingType>(request.RatingType!);

        // Get operations
        var operationId = _copilotIdService.DecodeId(request.Id!);
        var operation = await _dbContext.CopilotOperations
            .FirstOrDefaultAsync(x => x.Id == operationId, cancellationToken);
        if (operation is null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id));
        }

        var operationEntityId = operation.EntityId;

        // Get rating
        var rating = await _dbContext.CopilotOperationRatings
            .FirstOrDefaultAsync(x =>
                x.UserId == user.EntityId &&
                x.OperationId == operationEntityId, cancellationToken);

        // Set current rating and prev rating value
        var prevRating = OperationRatingType.None;
        OperationRatingType currentRating;
        if (rating is null)
        {
            // If rating in DB is null, then set prev as None, current as requested
            _dbContext.CopilotOperationRatings.Add(new CopilotOperationRating(
                operationEntityId, user.EntityId, ratingType));
            currentRating = ratingType;
        }
        else
        {
            // If rating in DB founded, set prev as what in db
            prevRating = rating.RatingType;

            // If prev is the same as requested, then set current to None
            // Else set current to what requested
            currentRating = rating.RatingType == ratingType ? OperationRatingType.None : ratingType;
            rating.ChangeRating(currentRating);
            _dbContext.CopilotOperationRatings.Update(rating);
        }

        // Check if need to do something with the operation Like and Dislike counter

        // ReSharper disable once ConvertIfStatementToSwitchStatement
        // LIKE -> STH. ELSE => Remove Like Count
        if (prevRating is OperationRatingType.Like && currentRating is not OperationRatingType.Like)
        {
            operation.RemoveLike(user.EntityId);
        }
        // DISLIKE -> STH. ELSE => Remove Dislike Count
        if (prevRating is OperationRatingType.Dislike && currentRating is not OperationRatingType.Dislike)
        {
            operation.RemoveDislike(user.EntityId);
        }
        // STH. ELSE -> LIKE => Add Like Count
        if (prevRating is not OperationRatingType.Like && currentRating is OperationRatingType.Like)
        {
            operation.AddLike(user.EntityId);
        }
        // STH. ELSE -> DISLIKE => Add Dislike Count
        if (prevRating is not OperationRatingType.Dislike && currentRating is OperationRatingType.Dislike)
        {
            operation.AddDislike(user.EntityId);
        }

        // Update database
        _dbContext.CopilotOperations.Update(operation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Return response
        return MaaApiResponseHelper.Ok(new RatingCopilotOperationDto
        {
            Id = request.Id!,
            RatingType = currentRating,
            CurrentRatio = operation.RatingRatio
        });
    }
}
