// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Constants;
using Microsoft.EntityFrameworkCore;
using ArkServerLanguage = MaaCopilotServer.GameData.Constants.ArkServerLanguage;

namespace MaaCopilotServer.Application.Arknights.GetDataVersion;

/// <summary>
///     The DTO for get data version query.
/// </summary>
public record GetDataVersionQuery : IRequest<MaaApiResponse>;

public class GetDataVersionQueryHandler : IRequestHandler<GetDataVersionQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;

    private static readonly ServerStatusDto s_syncNoError = new(string.Empty);
    private static readonly ServerStatusDto s_syncDisaster = new(SystemConstants.ARK_ASSET_CACHE_ERROR_DISASTER);

    private static readonly Func<string, ServerStatusDto> s_buildSyncErrorDto = (str) =>
    {
        var languages = str.Split(";");

        var dto = new ServerStatusDto(string.Empty);

        foreach (var language in languages)
        {
            var lang = Enum.Parse<ArkServerLanguage>(language);
            switch (lang)
            {
                case ArkServerLanguage.ChineseSimplified:
                    dto.ChineseSimplified = SystemConstants.ARK_ASSET_CACHE_ERROR_NORMAL;
                    break;
                case ArkServerLanguage.ChineseTraditional:
                    dto.ChineseTraditional = SystemConstants.ARK_ASSET_CACHE_ERROR_NORMAL;
                    break;
                case ArkServerLanguage.Korean:
                    dto.Korean = SystemConstants.ARK_ASSET_CACHE_ERROR_NORMAL;
                    break;
                case ArkServerLanguage.English:
                    dto.English = SystemConstants.ARK_ASSET_CACHE_ERROR_NORMAL;
                    break;
                case ArkServerLanguage.Japanese:
                    dto.Japanese = SystemConstants.ARK_ASSET_CACHE_ERROR_NORMAL;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return dto;
    };

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
        var twVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_TW, cancellationToken);
        var enVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_EN, cancellationToken);
        var jpVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_JP, cancellationToken);
        var koVersion = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_VERSION_KO, cancellationToken);

        var error = await _dbContext.PersistStorage
            .FirstOrDefaultAsync(x => x.Key == SystemConstants.ARK_ASSET_CACHE_ERROR, cancellationToken);
        
        var dto = new GetDataVersionQueryDto
        {
            LevelVersion = levelVersion.IsNotNull().Value,
            ServerVersion = new ServerStatusDto(
                cnVersion.IsNotNull().Value,
                twVersion.IsNotNull().Value,
                enVersion.IsNotNull().Value,
                jpVersion.IsNotNull().Value,
                koVersion.IsNotNull().Value),
            ServerSyncStatus = error is null
                ? s_syncNoError
                : error.Value == SystemConstants.ARK_ASSET_CACHE_ERROR_DISASTER
                    ? s_syncDisaster
                    : s_buildSyncErrorDto.Invoke(error.Value)
        };

        return MaaApiResponseHelper.Ok(dto);
    }
}
