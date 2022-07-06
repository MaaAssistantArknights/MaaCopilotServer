// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

// ReSharper disable InconsistentNaming

using MaaCopilotServer.GameData.Constants;

namespace MaaCopilotServer.Domain.Constants;

/// <summary>
///     System constants.
/// </summary>
public static class SystemConstants
{
    // DO NOT CHANGE THESE VALUES!

    public const string HOT_LIKE_MULTIPLIER = "oper.hot.multiplier.like";
    public const string HOT_DISLIKE_MULTIPLIER = "oper.hot.multiplier.dislike";
    public const string HOT_VIEW_MULTIPLIER = "oper.hot.multiplier.view";
    public const string HOT_INITIAL_SCORE = "oper.hot.initial";

    public const string ARK_ASSET_VERSION_LEVEL = "ark.asset.version.level";
    public const string ARK_ASSET_VERSION_CN = "ark.asset.version.cn";
    public const string ARK_ASSET_VERSION_EN = "ark.asset.version.en";
    public const string ARK_ASSET_VERSION_JP = "ark.asset.version.jp";
    public const string ARK_ASSET_VERSION_KO = "ark.asset.version.ko";

    public const string MaaCopilotSchemaPath = "static/maa-copilot-schema.json";

    public const string LevelUrl = "https://raw.githubusercontent.com/yuanyan3060/Arknights-Bot-Resource/main/levels.json";
    public const string ActUrl = "https://raw.githubusercontent.com/Kengxxiao/ArknightsGameData/master/{REGION}/gamedata/excel/activity_table.json";
    public const string CharUrl = "https://raw.githubusercontent.com/Kengxxiao/ArknightsGameData/master/{REGION}/gamedata/excel/character_table.json";
    public const string StageUrl = "https://raw.githubusercontent.com/Kengxxiao/ArknightsGameData/master/{REGION}/gamedata/excel/stage_table.json";
    public const string ZoneUrl = "https://raw.githubusercontent.com/Kengxxiao/ArknightsGameData/master/{REGION}/gamedata/excel/zone_table.json";

    public const string ArkLevelCommit = "https://api.github.com/repos/yuanyan3060/Arknights-Bot-Resource/commits?path=levels.json&per_page=1";
    public const string ArkDataCommit = "https://api.github.com/repos/Kengxxiao/ArknightsGameData/commits?path={REGION}&per_page=1";

    public static readonly Dictionary<string, ArkServerLanguage> ArkServerRegions = new()
    {
        { "zh_CN", ArkServerLanguage.Chinese },
        { "en_US", ArkServerLanguage.English },
        { "ja_JP", ArkServerLanguage.Japanese },
        { "ko_KR", ArkServerLanguage.Korean },
    };
}
