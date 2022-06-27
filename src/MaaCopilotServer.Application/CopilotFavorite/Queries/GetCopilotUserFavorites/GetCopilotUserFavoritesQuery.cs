// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotFavorite.Queries.GetCopilotUserFavorites;

[Authorized(UserRole.User)]
public record GetCopilotUserFavoritesQuery : IRequest<MaaApiResponse>
{
    [FromQuery(Name = "id")] public string? FavoriteListId { get; set; }
}

public class GetCopilotUserFavoritesQueryHandler : IRequestHandler<GetCopilotUserFavoritesQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;

    public GetCopilotUserFavoritesQueryHandler(
        ICopilotIdService copilotIdService,
        ICurrentUserService currentUserService,
        IMaaCopilotDbContext dbContext)
    {
        _copilotIdService = copilotIdService;
        _currentUserService = currentUserService;
        _dbContext = dbContext;
    }

    public async Task<MaaApiResponse> Handle(GetCopilotUserFavoritesQuery request,
        CancellationToken cancellationToken)
    {
        var user = (await _currentUserService.GetUser()).IsNotNull();
        var favListId = Guid.Parse(request.FavoriteListId!);

        var list = await _dbContext.CopilotUserFavorites
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted == false)
            .Include(x => x.Operations)
            .ThenInclude(x => x.Author)
            .FirstOrDefaultAsync(x => x.EntityId == favListId, cancellationToken);
        if (list is null)
        {
            return MaaApiResponseHelper.NotFound("");
        }

        // TODO: Find more elegant way to do this.
        var rating = (await _dbContext.CopilotOperationRatings
                .Where(x => x.UserId == user.EntityId)
                .ToListAsync(cancellationToken))
            .Where(x => list.Operations.Any(y => y.EntityId == x.OperationId))
            .ToList();

        var operationsDto = list.Operations
            .Where(x => x.IsDeleted == false)
            .Select(x => new FavoriteCopilotOperationsDto
            {
                Id = _copilotIdService.EncodeId(x.Id),
                StageName = x.StageName,
                MinimumRequired = x.MinimumRequired,
                Detail = x.Details,
                Operators = x.Operators,
                Title = x.Title,
                Uploader = x.Author.UserName,
                UploadTime = x.CreateAt.ToIsoString(),
                ViewCounts = x.ViewCounts,
                RatingRatio = x.RatingRatio,
                Groups = x.Groups.ToArray().DeserializeGroup(),
                RatingType = rating.FirstOrDefault(y => y.OperationId == x.EntityId)?
                    .RatingType ?? OperationRatingType.None
            }).ToList();
        var operationsDtoDeleted = list.Operations
            .Where(x => x.IsDeleted)
            .Select(x => new FavoriteCopilotOperationsDto
            {
                Id = _copilotIdService.EncodeId(x.Id),
                Deleted = true
            });
        operationsDto.AddRange(operationsDtoDeleted);
        var dto = new GetCopilotUserFavoritesDto(list.EntityId.ToString(), list.FavoriteName, operationsDto);
        return MaaApiResponseHelper.Ok(dto);
    }
}
