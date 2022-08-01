// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.GameData.Entity;

public class ArkLevelEntityGlobal
{
    internal ArkLevelEntityGlobal(ArkLevelEntity cn, ArkLevelEntity? cnT, ArkLevelEntity? en, ArkLevelEntity? jp, ArkLevelEntity? ko)
    {
        Name = new ArkI18N(cn.Name, cnT?.Name, en?.Name, jp?.Name, ko?.Name);
        CatOne = new ArkI18N(cn.CatOne, cnT?.CatOne, en?.CatOne, jp?.CatOne, ko?.CatOne);
        CatTwo = new ArkI18N(cn.CatTwo, cnT?.CatTwo, en?.CatTwo, jp?.CatTwo, ko?.CatTwo);
        CatThree = new ArkI18N(cn.CatThree, cnT?.CatThree, en?.CatThree, jp?.CatThree, ko?.CatThree);
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
        Name = new ArkI18N($"{id}CN", $"{id}TW",$"{id}EN", $"{id}JP", $"{id}KO");
        CatOne = new ArkI18N($"{id}CatOneCN", $"{id}CatOneTW", $"{id}CatOneEN", $"{id}CatOneJP", $"{id}CatOneKO");
        CatTwo = new ArkI18N($"{id}CatTwoCN", $"{id}CatTwoTW", $"{id}CatTwoEN", $"{id}CatTwoJP", $"{id}CatTwoKO");
        CatThree = new ArkI18N($"{id}CatThreeCN", $"{id}CatThreeTW", $"{id}CatThreeEN", $"{id}CatThreeJP", $"{id}CatThreeKO");
        Width = 100;
        Height = 100;
    }

    public ArkI18N Name { get; }
    public ArkI18N CatOne { get; }
    public ArkI18N CatTwo { get; }
    public ArkI18N CatThree { get; }

    public string LevelId { get; }
    public int Width { get; } = 0;
    public int Height { get; } = 0;
}
