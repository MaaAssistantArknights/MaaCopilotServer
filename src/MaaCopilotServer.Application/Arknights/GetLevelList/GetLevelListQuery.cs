// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Arknights.GetLevelList;

/// <summary>
///     The DTO for the get level list query.
/// </summary>
public record GetLevelListQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The server language.
    /// <para>Options: </para>
    /// <para>Chinese (China Mainland) - zh_cn, cn</para>
    /// <para>Chinese (Taiwan, China) - zh_tw, tw</para>
    /// <para>English (Global) - en_us, en</para>
    /// <para>Japanese (Japan) - ja_jp, ja</para>
    /// <para>Korean (South Korea) - ko_kr, ko</para>
    /// </summary>
    [FromQuery(Name = "language")]
    public string Language { get; set; } = string.Empty;
}

public class GetLevelListQueryHandler : IRequestHandler<GetLevelListQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;

    public GetLevelListQueryHandler(IMaaCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MaaApiResponse> Handle(GetLevelListQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.ArkLevelData
            .Include(x => x.Name)
            .Include(x => x.CatOne)
            .Include(x => x.CatTwo)
            .Include(x => x.CatThree)
            .AsQueryable();

        var qFunc = request.Language.GetLevelQueryFunc();
        var mFunc = request.Language.GetLevelMapperFunc();

        query = qFunc.Invoke(query);

        var data = await query.ToListAsync(cancellationToken);
        var dto = data.Select(mFunc);
        return MaaApiResponseHelper.Ok(dto);
    }
}
