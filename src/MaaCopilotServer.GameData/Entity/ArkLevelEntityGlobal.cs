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
    public ArkLevelEntityGlobal(string? levelId = null)
    {
        var id = levelId ?? string.Empty;
        LevelId = id;
        NameCn = id + "CN";
        NameEn = id + "EN";
        NameJp = id + "JP";
        NameKo = id + "KO";
        CatOneCn = id + "CatOneCN";
        CatOneEn = id + "CatOneEN";
        CatOneJp = id + "CatOneJP";
        CatOneKo = id + "CatOneKO";
        CatTwoCn = id + "CatTwoCN";
        CatTwoEn = id + "CatTwoEN";
        CatTwoJp = id + "CatTwoJP";
        CatTwoKo = id + "CatTwoKO";
        CatThreeCn = id + "CatThreeCN";
        CatThreeEn = id + "CatThreeEN";
        CatThreeJp = id + "CatThreeJP";
        CatThreeKo = id + "CatThreeKO";
        Width = 100;
        Height = 100;
    }

    public string NameCn { get; }
    public string NameKo { get; }
    public string NameJp { get; }
    public string NameEn { get; }

    public string CatOneCn { get; }
    public string CatOneKo { get; }
    public string CatOneJp { get; }
    public string CatOneEn { get; }

    public string CatTwoCn { get; }
    public string CatTwoKo { get; }
    public string CatTwoJp { get; }
    public string CatTwoEn { get; }

    public string CatThreeCn { get; }
    public string CatThreeKo { get; }
    public string CatThreeJp { get; }
    public string CatThreeEn { get; }

    public string LevelId { get; }
    public int Width { get; } = 0;
    public int Height { get; } = 0;
}
