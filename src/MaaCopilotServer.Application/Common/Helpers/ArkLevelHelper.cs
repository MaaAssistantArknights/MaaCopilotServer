// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Arknights.GetLevelList;
using MaaCopilotServer.Domain.Entities;

namespace MaaCopilotServer.Application.Common.Helpers;

public static class ArkLevelHelper
{
    public static GetLevelListDto MapToDto(this ArkLevelData data, string? server) => server?.ToLower() switch
    {
        null => s_mapCn(data),
        "english" => s_mapEn(data),
        "japanese" => s_mapJp(data),
        "korean" => s_mapKo(data),
        _ => s_mapCn(data)
    };

    private static readonly Func<ArkLevelData, GetLevelListDto> s_mapCn = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOneCn,
                CatTwo = data.CatTwoCn,
                CatThree = data.CatThreeCn,
                Name = data.NameCn,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };

    private static readonly Func<ArkLevelData, GetLevelListDto> s_mapEn = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOneEn,
                CatTwo = data.CatTwoEn,
                CatThree = data.CatThreeEn,
                Name = data.NameEn,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };

    private static readonly Func<ArkLevelData, GetLevelListDto> s_mapJp = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOneJp,
                CatTwo = data.CatTwoJp,
                CatThree = data.CatThreeJp,
                Name = data.NameJp,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };

    private static readonly Func<ArkLevelData, GetLevelListDto> s_mapKo = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOneKo,
                CatTwo = data.CatTwoKo,
                CatThree = data.CatThreeKo,
                Name = data.NameKo,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };
}
