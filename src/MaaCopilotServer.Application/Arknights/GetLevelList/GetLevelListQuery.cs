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
    ///     The server language. Could be (ignore case) Chinese (cn), English (en), Japanese (ja), Korean (ko).
    /// </summary>
    [FromQuery(Name = "server")]
    public string Server { get; set; } = string.Empty;
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
        var query = _dbContext.ArkLevelData.AsQueryable();

        var qFunc = request.Server.GetLevelQueryFunc();
        var mFunc = request.Server.GetLevelMapperFunc();

        query = qFunc.Invoke(query);

        var data = await query.ToListAsync(cancellationToken);
        var dto = data.Select(mFunc);
        return MaaApiResponseHelper.Ok(dto);
    }
}
