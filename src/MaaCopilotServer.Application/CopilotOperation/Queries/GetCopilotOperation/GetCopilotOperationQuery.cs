// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Operation;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
///     The DTO for the GetCopilotOperation query.
/// </summary>
public record GetCopilotOperationQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The operation id.
    /// </summary>
    [Required]
    public string? Id { get; set; }

    /// <summary>
    ///     The server language.
    /// <para>Options: </para>
    /// <para>Chinese (China Mainland) - zh_cn, cn</para>
    /// <para>Chinese (Taiwan, China) - zh_tw, tw</para>
    /// <para>English (Global) - en_us, en</para>
    /// <para>Japanese (Japan) - ja_jp, ja</para>
    /// <para>Korean (South Korea) - ko_kr, ko</para>
    /// </summary>
    public string? Language { get; set; } = string.Empty;
}

public class
    GetCopilotOperationQueryHandler : IRequestHandler<GetCopilotOperationQuery,
        MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICopilotOperationService _copilotOperationService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetCopilotOperationQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ICopilotOperationService copilotOperationService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _copilotOperationService = copilotOperationService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(GetCopilotOperationQuery request,
        CancellationToken cancellationToken)
    {
        // Get current infos
        var user = await _currentUserService.GetUser();
        var isLoggedIn = user is not null;

        // Get operation
        var id = EntityIdHelper.DecodeId(request.Id!);
        var entity = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Include(x => x.ArkLevel).ThenInclude(x => x.Name)
            .Include(x => x.ArkLevel).ThenInclude(x => x.CatOne)
            .Include(x => x.ArkLevel).ThenInclude(x => x.CatTwo)
            .Include(x => x.ArkLevel).ThenInclude(x => x.CatThree)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return MaaApiResponseHelper.NotFound(string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id));
        }

        // If the user is not logged in, the operation rating type in response will be set to null
        // If the user is logged in, the operation rating type in response will be set to the user's rating type
        // and query the database to get the user's rating for the operation
        OperationRatingType? rating = isLoggedIn
            ? (await _dbContext.CopilotOperationRatings
                .FirstOrDefaultAsync(x => x.UserId == user!.EntityId
                                          && x.OperationId == entity.EntityId, cancellationToken))?
                .RatingType ?? OperationRatingType.None
            : null;

        // Add view count and update hot score
        // Do this before build dto, so user can see their action
        entity.AddViewCount();
        entity.UpdateHotScore(_copilotOperationService.CalculateHotScore(entity));

        // Build dto
        var dto = new GetCopilotOperationQueryDto
        {
            Id = request.Id!,
            MinimumRequired = entity.MinimumRequired,
            Content = entity.Content,
            Detail = entity.Details,
            Operators = entity.Operators,
            Title = entity.Title,
            Uploader = entity.Author.UserName,
            UploadTime = entity.CreateAt.ToIsoString(),
            ViewCounts = entity.ViewCounts,
            HotScore = entity.HotScore,
            Level = request.Language.GetLevelMapperFunc().Invoke(entity.ArkLevel),
            Groups = entity.Groups.ToArray().DeserializeGroup(),
            RatingLevel = entity.RatingLevel,
            RatingRatio = entity.RatingRatio,
            IsNotEnoughRating = entity.IsNotEnoughRating,
            RatingType = rating,
            Difficulty = entity.Difficulty,
        };

        // Add an view count to the operation and update it in the database
        _dbContext.CopilotOperations.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok(dto);
    }
}
