// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUserFavorites;

public record GetCopilotUserFavoritesQuery : IRequest<MaaActionResult<GetCopilotUserFavoritesDto>>
{
    [FromQuery(Name = "id")]
    public string? FavoriteListId { get; set; }
}

public class GetCopilotUserFavoritesQueryHandler :
    IRequestHandler<GetCopilotUserFavoritesQuery,
        MaaActionResult<GetCopilotUserFavoritesDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICopilotIdService _copilotIdService;
    private readonly IMaaCopilotDbContext _copilotDbContext;

    public GetCopilotUserFavoritesQueryHandler(
        ICurrentUserService currentUserService,
        ICopilotIdService copilotIdService,
        IMaaCopilotDbContext copilotDbContext)
    {
        _currentUserService = currentUserService;
        _copilotIdService = copilotIdService;
        _copilotDbContext = copilotDbContext;
    }

    public async Task<MaaActionResult<GetCopilotUserFavoritesDto>> Handle(GetCopilotUserFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favListId = Guid.Parse(request.FavoriteListId!);

        var list = await _copilotDbContext.CopilotUserFavorites
            .Include(x => x.Operations)
            .ThenInclude(x => x.Author)
            .FirstOrDefaultAsync(x => x.EntityId == favListId, cancellationToken);
        if (list is null)
        {
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(), ""));
        }

        var operationsDto = list.Operations.Select(x => new QueryCopilotOperationsQueryDto(
            _copilotIdService.EncodeId(x.Id), x.StageName, x.MinimumRequired, x.CreateAt.ToIsoString(),
            x.Author.UserName, x.Title, x.Details, x.Downloads, x.Operators)).ToList();
        var dto = new GetCopilotUserFavoritesDto(list.EntityId.ToString(), list.FavoriteName, operationsDto);
        return MaaApiResponse.Ok(dto, _currentUserService.GetTrackingId());
    }
}
