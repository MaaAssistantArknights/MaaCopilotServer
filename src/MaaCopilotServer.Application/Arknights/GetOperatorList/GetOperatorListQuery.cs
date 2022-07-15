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
    ///     The server language. Could be (ignore case) Chinese (cn), English (en), Japanese (ja), Korean (ko).
    /// </summary>
    [FromQuery(Name = "server")]
    public string Server { get; set; } = string.Empty;
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
        var query = _dbContext.ArkCharacterInfos.AsQueryable();

        var qFunc = request.Server.GetCharQueryFunc();
        var mFunc = request.Server.GetCharMapperFunc();

        query = qFunc.Invoke(query);

        var data = await query.ToListAsync(cancellationToken);
        var dto = data.Select(mFunc);
        return MaaApiResponseHelper.Ok(dto);
    }
}
