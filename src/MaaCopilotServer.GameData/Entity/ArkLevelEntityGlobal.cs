// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.GameData.Entity;

public class ArkLevelEntityGlobal
{
    internal ArkLevelEntityGlobal(ArkLevelEntity cn, ArkLevelEntity? en, ArkLevelEntity? jp, ArkLevelEntity? ko)
    {
        NameCn = cn.Name;
        NameEn = en?.Name ?? string.Empty;
        NameJp = jp?.Name ?? string.Empty;
        NameKo = ko?.Name ?? string.Empty;

        CatOneCn = cn.CatOne;
        CatOneEn = en?.CatOne ?? string.Empty;
        CatOneJp = jp?.CatOne ?? string.Empty;
        CatOneKo = ko?.CatOne ?? string.Empty;

        CatTwoCn = cn.CatTwo;
        CatTwoEn = en?.CatTwo ?? string.Empty;
        CatTwoJp = jp?.CatTwo ?? string.Empty;
        CatTwoKo = ko?.CatTwo ?? string.Empty;

        CatThreeCn = cn.CatThree;
        CatThreeEn = en?.CatThree ?? string.Empty;
        CatThreeJp = jp?.CatThree ?? string.Empty;
        CatThreeKo = ko?.CatThree ?? string.Empty;

        LevelId = cn.LevelId;
        Width = cn.Width;
        Height = cn.Height;
    }

    /// <summary>
    ///     FOR TEST ONLY
    /// </summary>
    public ArkLevelEntityGlobal() { }

    public string NameCn { get; } = string.Empty;
    public string NameKo { get; } = string.Empty;
    public string NameJp { get; } = string.Empty;
    public string NameEn { get; } = string.Empty;

    public string CatOneCn { get; } = string.Empty;
    public string CatOneKo { get; } = string.Empty;
    public string CatOneJp { get; } = string.Empty;
    public string CatOneEn { get; } = string.Empty;

    public string CatTwoCn { get; } = string.Empty;
    public string CatTwoKo { get; } = string.Empty;
    public string CatTwoJp { get; } = string.Empty;
    public string CatTwoEn { get; } = string.Empty;

    public string CatThreeCn { get; } = string.Empty;
    public string CatThreeKo { get; } = string.Empty;
    public string CatThreeJp { get; } = string.Empty;
    public string CatThreeEn { get; } = string.Empty;

    public string LevelId { get; } = string.Empty;
    public int Width { get; } = 0;
    public int Height { get; } = 0;
}
