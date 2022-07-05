// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Constants;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Arknights.GetDataVersion;

/// <summary>
///     The DTO for get data version query.
/// </summary>
public record GetDataVersionQuery : IRequest<MaaApiResponse>;

public class GetDataVersionQueryHandler : IRequestHandler<GetDataVersionQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;

    public GetDataVersionQueryHandler(IMaaCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MaaApiResponse> Handle(GetDataVersionQuery request, CancellationToken cancellationToken)
    {
        var levelVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_LEVEL, cancellationToken);
        var cnVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_CN, cancellationToken);
        var enVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_EN, cancellationToken);
        var jpVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_JP, cancellationToken);
        var koVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_KO, cancellationToken);

        var dto = new GetDataVersionQueryDto
        {
            LevelVersion = levelVersion.IsNotNull().Value,
            CnVersion = cnVersion.IsNotNull().Value,
            EnVersion = enVersion.IsNotNull().Value,
            JpVersion = jpVersion.IsNotNull().Value,
            KoVersion = koVersion.IsNotNull().Value
        };

        return MaaApiResponseHelper.Ok(dto);
    }
}
