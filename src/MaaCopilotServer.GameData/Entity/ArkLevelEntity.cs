// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.GameData.Model;

namespace MaaCopilotServer.GameData.Entity;

internal class ArkLevelEntity
{
    internal ArkLevelEntity(string catOne, string catTwo, string catThree, ArkLevel level)
    {
        CatOne = catOne.Trim();
        CatTwo = catTwo.Trim();
        CatThree = catThree.Trim();
        LevelId = level.LevelId.Trim();
        Width = level.Width;
        Height = level.Height;
        Name = level.Name.Trim();
    }

    internal string Name { get; set; }
    internal string CatOne { get; set; }
    internal string CatTwo { get; set; }
    internal string CatThree { get; set; }
    internal string LevelId { get; set; }
    internal int Width { get; set; }
    internal int Height { get; set; }
}
