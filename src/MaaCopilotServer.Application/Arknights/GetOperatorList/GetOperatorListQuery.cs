// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Arknights.GetOperatorList;

/// <summary>
///     The DTO for the get operation list query.
/// </summary>
public record GetOperatorListQuery : IRequest<MaaApiResponse>
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


public class GetOperatorListQueryHandler : IRequestHandler<GetOperatorListQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;

    public GetOperatorListQueryHandler(IMaaCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MaaApiResponse> Handle(GetOperatorListQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext
            .ArkCharacterInfos
            .Include(x => x.Name)
            .AsQueryable();

        var qFunc = request.Language.GetCharQueryFunc();
        var mFunc = request.Language.GetCharMapperFunc();

        query = qFunc.Invoke(query);

        var data = await query.ToListAsync(cancellationToken);
        var dto = data.Select(mFunc);
        return MaaApiResponseHelper.Ok(dto);
    }
}
