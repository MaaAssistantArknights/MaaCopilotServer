// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.GameData.Entity;

namespace MaaCopilotServer.Domain.Entities;

/// <summary>
///     Ark level info.
/// </summary>
public class ArkLevelData : BaseEntity
{
#pragma warning disable CS8618
    public ArkLevelData(ArkLevelEntityGlobal level)
    {
        Update(level);
        CreateBy = Guid.Empty;
    }
    public ArkLevelData(Guid creator)
    {
        CreateBy = creator;
    }
    
    // ReSharper disable once UnusedMember.Local
    public ArkLevelData() { }
#pragma warning restore CS8618

    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

    public ArkI18N Name { get; set; } = new();
    public ArkI18N CatOne { get; set; } = new();
    public ArkI18N CatTwo { get; set; } = new();
    public ArkI18N CatThree { get; set; } = new();
    public ArkI18N? Keyword { get; set; }

    public string LevelId { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public bool Custom { get; set; }

    public void Update(ArkLevelEntityGlobal level)
    {
        Name.Update(level.Name);
        CatOne.Update(level.CatOne);
        CatTwo.Update(level.CatTwo);
        CatThree.Update(level.CatThree);

        Keyword ??= new ArkI18N();
        Keyword.Update(level.Keyword);

        LevelId = level.LevelId;
        Width = level.Width;
        Height = level.Height;
    }

    public bool IsEqual(ArkLevelEntityGlobal level)
    {
        return Name.IsEqual(level.Name) &&
               LevelId == level.LevelId &&
               Width == level.Width &&
               Height == level.Height &&
               Keyword is not null &&
               Keyword.IsEqual(level.Keyword);
    }
}
