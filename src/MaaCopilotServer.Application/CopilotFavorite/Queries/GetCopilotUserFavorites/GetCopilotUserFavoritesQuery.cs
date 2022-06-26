// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotFavorite.Queries.GetCopilotUserFavorites;

public record GetCopilotUserFavoritesQuery : IRequest<MaaApiResponse>
{
    [FromQuery(Name = "id")] public string? FavoriteListId { get; set; }
}

public class GetCopilotUserFavoritesQueryHandler : IRequestHandler<GetCopilotUserFavoritesQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _copilotDbContext;
    private readonly ICopilotIdService _copilotIdService;

    public GetCopilotUserFavoritesQueryHandler(
        ICopilotIdService copilotIdService,
        IMaaCopilotDbContext copilotDbContext)
    {
        _copilotIdService = copilotIdService;
        _copilotDbContext = copilotDbContext;
    }

    public async Task<MaaApiResponse> Handle(GetCopilotUserFavoritesQuery request,
        CancellationToken cancellationToken)
    {
        var favListId = Guid.Parse(request.FavoriteListId!);

        var list = await _copilotDbContext.CopilotUserFavorites
            .IgnoreQueryFilters()
            .Where(x => x.IsDeleted == false)
            .Include(x => x.Operations)
            .ThenInclude(x => x.Author)
            .FirstOrDefaultAsync(x => x.EntityId == favListId, cancellationToken);
        if (list is null)
        {
            return MaaApiResponseHelper.NotFound("");
        }

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
