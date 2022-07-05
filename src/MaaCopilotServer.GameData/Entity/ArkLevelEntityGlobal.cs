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

    public string NameCn { get; set; }
    public string NameKo { get; set; }
    public string NameJp { get; set; }
    public string NameEn { get; set; }

    public string CatOneCn { get; set; }
    public string CatOneKo { get; set; }
    public string CatOneJp { get; set; }
    public string CatOneEn { get; set; }

    public string CatTwoCn { get; set; }
    public string CatTwoKo { get; set; }
    public string CatTwoJp { get; set; }
    public string CatTwoEn { get; set; }

    public string CatThreeCn { get; set; }
    public string CatThreeKo { get; set; }
    public string CatThreeJp { get; set; }
    public string CatThreeEn { get; set; }

    public string LevelId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}
