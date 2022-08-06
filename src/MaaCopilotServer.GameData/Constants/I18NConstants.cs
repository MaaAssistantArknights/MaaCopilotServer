// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.GameData.Constants;

internal static class I18NConstants
{
    #region Character Profression

    public static string GetCharacterProfessionI18NString(this CharacterProfessions professions, ArkServerLanguage language)
        => language switch
        {
            ArkServerLanguage.ChineseSimplified => professions.GetCharacterProfessionCnString(),
            ArkServerLanguage.ChineseTraditional => professions.GetCharacterProfessionCnTString(),
            ArkServerLanguage.Korean => professions.GetCharacterProfessionKoString(),
            ArkServerLanguage.English => professions.GetCharacterProfessionEnString(),
            ArkServerLanguage.Japanese => professions.GetCharacterProfessionJpString(),
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };

    private static string GetCharacterProfessionCnString(this CharacterProfessions professions) => professions switch
    {
        CharacterProfessions.Medic => "医疗",
        CharacterProfessions.Special => "特种",
        CharacterProfessions.Warrior => "近卫",
        CharacterProfessions.Sniper => "狙击",
        CharacterProfessions.Tank => "重装",
        CharacterProfessions.Caster => "术士",
        CharacterProfessions.Pioneer => "先锋",
        CharacterProfessions.Support => "辅助",
        CharacterProfessions.Unknown => "未知",
        _ => throw new ArgumentOutOfRangeException(nameof(professions), professions, null)
    };

    private static string GetCharacterProfessionCnTString(this CharacterProfessions professions) => professions switch
    {
        CharacterProfessions.Medic => "醫療",
        CharacterProfessions.Special => "特種",
        CharacterProfessions.Warrior => "近衛",
        CharacterProfessions.Sniper => "狙撃",
        CharacterProfessions.Tank => "重装",
        CharacterProfessions.Caster => "術師",
        CharacterProfessions.Pioneer => "先鋒",
        CharacterProfessions.Support => "輔助",
        CharacterProfessions.Unknown => "未知",
        _ => throw new ArgumentOutOfRangeException(nameof(professions), professions, null)
    };

    private static string GetCharacterProfessionEnString(this CharacterProfessions professions) => professions switch
    {
        CharacterProfessions.Medic => "Medic",
        CharacterProfessions.Special => "Specialist",
        CharacterProfessions.Warrior => "Guard",
        CharacterProfessions.Sniper => "Sniper",
        CharacterProfessions.Tank => "Defender",
        CharacterProfessions.Caster => "Caster",
        CharacterProfessions.Pioneer => "Vanguard",
        CharacterProfessions.Support => "Supporter",
        CharacterProfessions.Unknown => "Unknown",
        _ => throw new ArgumentOutOfRangeException(nameof(professions), professions, null)
    };

    private static string GetCharacterProfessionJpString(this CharacterProfessions professions) => professions switch
    {
        CharacterProfessions.Medic => "医療",
        CharacterProfessions.Special => "特殊",
        CharacterProfessions.Warrior => "前衛",
        CharacterProfessions.Sniper => "狙撃",
        CharacterProfessions.Tank => "重装",
        CharacterProfessions.Caster => "術師",
        CharacterProfessions.Pioneer => "先鋒",
        CharacterProfessions.Support => "補助",
        CharacterProfessions.Unknown => "不明",
        _ => throw new ArgumentOutOfRangeException(nameof(professions), professions, null)
    };

    private static string GetCharacterProfessionKoString(this CharacterProfessions professions) => professions switch
    {
        CharacterProfessions.Medic => "메딕",
        CharacterProfessions.Special => "스페셜리스트",
        CharacterProfessions.Warrior => "가드",
        CharacterProfessions.Sniper => "스나이퍼",
        CharacterProfessions.Tank => "디펜더",
        CharacterProfessions.Caster => "캐스터",
        CharacterProfessions.Pioneer => "뱅가드",
        CharacterProfessions.Support => "서포터",
        CharacterProfessions.Unknown => "알려지지 않은",
        _ => throw new ArgumentOutOfRangeException(nameof(professions), professions, null)
    };

    #endregion

    #region Zone Type

    public static string GetZoneTypeI18NString(this ZoneTypes zoneTypes, ArkServerLanguage language)
        => language switch
        {
            ArkServerLanguage.ChineseSimplified => zoneTypes.GetZoneTypeCnString(),
            ArkServerLanguage.ChineseTraditional => zoneTypes.GetZoneTypeCnTString(),
            ArkServerLanguage.Korean => zoneTypes.GetZoneTypeKoString(),
            ArkServerLanguage.English => zoneTypes.GetZoneTypeEnString(),
            ArkServerLanguage.Japanese => zoneTypes.GetZoneTypeJpString(),
            _ => throw new ArgumentOutOfRangeException(nameof(language), language, null)
        };

    private static string GetZoneTypeCnString(this ZoneTypes zoneTypes) => zoneTypes switch
    {
        ZoneTypes.MainLine => "主题曲",
        ZoneTypes.Weekly => "物资筹备 & 芯片搜索",
        ZoneTypes.Activity => "活动关卡",
        ZoneTypes.Campaign => "剿灭作战",
        ZoneTypes.Memory => "悖論模擬",
        _ => throw new ArgumentOutOfRangeException(nameof(zoneTypes), zoneTypes, null)
    };

    private static string GetZoneTypeCnTString(this ZoneTypes zoneTypes) => zoneTypes switch
    {
        ZoneTypes.MainLine => "主題曲",
        ZoneTypes.Weekly => "物資籌備 & 芯片搜索",
        ZoneTypes.Activity => "活動關卡",
        ZoneTypes.Campaign => "剿滅作戰",
        ZoneTypes.Memory => "悖論模擬",
        _ => throw new ArgumentOutOfRangeException(nameof(zoneTypes), zoneTypes, null)
    };

    private static string GetZoneTypeEnString(this ZoneTypes zoneTypes) => zoneTypes switch
    {
        ZoneTypes.MainLine => "Main Theme",
        ZoneTypes.Weekly => "Supply operations",
        ZoneTypes.Activity => "Event operations",
        ZoneTypes.Campaign => "Annihilation",
        ZoneTypes.Memory => "Paradox Simulation",
        _ => throw new ArgumentOutOfRangeException(nameof(zoneTypes), zoneTypes, null)
    };

    private static string GetZoneTypeJpString(this ZoneTypes zoneTypes) => zoneTypes switch
    {
        ZoneTypes.MainLine => "メインテーマ",
        ZoneTypes.Weekly => "資源調達 と SoC捜索",
        ZoneTypes.Activity => "活動レベル",
        ZoneTypes.Campaign => "殲滅作戦",
        ZoneTypes.Memory => "逆理演算",
        _ => throw new ArgumentOutOfRangeException(nameof(zoneTypes), zoneTypes, null)
    };

    private static string GetZoneTypeKoString(this ZoneTypes zoneTypes) => zoneTypes switch
    {
        ZoneTypes.MainLine => "메인",
        ZoneTypes.Weekly => "물자 비축 & 칩탐색",
        ZoneTypes.Activity => "이벤트",
        ZoneTypes.Campaign => "섬멸 작전",
        ZoneTypes.Memory => "패러독스 시뮬레이션",
        _ => throw new ArgumentOutOfRangeException(nameof(zoneTypes), zoneTypes, null)
    };

    #endregion

    #region Mainline Level Difficulty

    public static string GetMainLineLevelDifficultyI18NString(this MainLineLevelDifficulty difficulty, ArkServerLanguage language)
        => language switch
        {
            ArkServerLanguage.ChineseSimplified => difficulty.GetMainLineLevelDifficultyCnString(),
            ArkServerLanguage.ChineseTraditional => difficulty.GetMainLineLevelDifficultyCnTString(),
            ArkServerLanguage.Korean => difficulty.GetMainLineLevelDifficultyKoString(),
            ArkServerLanguage.English => difficulty.GetMainLineLevelDifficultyEnString(),
            ArkServerLanguage.Japanese => difficulty.GetMainLineLevelDifficultyJpString(),
            _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
        };

    private static string GetMainLineLevelDifficultyCnString(this MainLineLevelDifficulty difficulty) => difficulty switch
    {
        MainLineLevelDifficulty.Easy => "简单",
        MainLineLevelDifficulty.Main => "标准",
        MainLineLevelDifficulty.Tough => "磨难",
        MainLineLevelDifficulty.Unknown => "标准",
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
    };

    private static string GetMainLineLevelDifficultyCnTString(this MainLineLevelDifficulty difficulty) => difficulty switch
    {
        MainLineLevelDifficulty.Easy => "簡單",
        MainLineLevelDifficulty.Main => "標準",
        MainLineLevelDifficulty.Tough => "磨難",
        MainLineLevelDifficulty.Unknown => "標準",
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
    };

    private static string GetMainLineLevelDifficultyEnString(this MainLineLevelDifficulty difficulty) => difficulty switch
    {
        MainLineLevelDifficulty.Easy => "Easy",
        MainLineLevelDifficulty.Main => "Standard",
        MainLineLevelDifficulty.Tough => "Tough",
        MainLineLevelDifficulty.Unknown => "Standard",
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
    };

    private static string GetMainLineLevelDifficultyJpString(this MainLineLevelDifficulty difficulty) => difficulty switch
    {
        MainLineLevelDifficulty.Easy => "簡単",
        MainLineLevelDifficulty.Main => "標準",
        MainLineLevelDifficulty.Tough => "困難",
        MainLineLevelDifficulty.Unknown => "標準",
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
    };

    private static string GetMainLineLevelDifficultyKoString(this MainLineLevelDifficulty difficulty) => difficulty switch
    {
        MainLineLevelDifficulty.Easy => "단순한",
        MainLineLevelDifficulty.Main => "기준",
        MainLineLevelDifficulty.Tough => "어려움",
        MainLineLevelDifficulty.Unknown => "기준",
        _ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
    };

    #endregion
}
