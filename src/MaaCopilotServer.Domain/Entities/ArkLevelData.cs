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
#pragma warning restore CS8618

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private ArkLevelData() { }
#pragma warning restore CS8618

    public string NameCn { get; private set; }
    public string NameKo { get; private set; }
    public string NameJp { get; private set; }
    public string NameEn { get; private set; }

    public string CatOneCn { get; private set; }
    public string CatOneKo { get; private set; }
    public string CatOneJp { get; private set; }
    public string CatOneEn { get; private set; }

    public string CatTwoCn { get; private set; }
    public string CatTwoKo { get; private set; }
    public string CatTwoJp { get; private set; }
    public string CatTwoEn { get; private set; }

    public string CatThreeCn { get; private set; }
    public string CatThreeKo { get; private set; }
    public string CatThreeJp { get; private set; }
    public string CatThreeEn { get; private set; }

    public string LevelId { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public void Update(ArkLevelEntityGlobal level)
    {
        NameCn = level.NameCn;
        NameEn = level.NameEn;
        NameJp = level.NameJp;
        NameKo = level.NameKo;

        CatOneCn = level.CatOneCn;
        CatOneEn = level.CatOneEn;
        CatOneJp = level.CatOneJp;
        CatOneKo = level.CatOneKo;

        CatTwoCn = level.CatTwoCn;
        CatTwoEn = level.CatTwoEn;
        CatTwoJp = level.CatTwoJp;
        CatTwoKo = level.CatTwoKo;

        CatThreeCn = level.CatThreeCn;
        CatThreeEn = level.CatThreeEn;
        CatThreeJp = level.CatThreeJp;
        CatThreeKo = level.CatThreeKo;

        LevelId = level.LevelId;
        Width = level.Width;
        Height = level.Height;
    }

    public bool IsEqual(ArkLevelEntityGlobal level)
    {
        return NameCn == level.NameCn &&
               NameEn == level.NameEn &&
               NameJp == level.NameJp &&
               NameKo == level.NameKo &&
               CatOneCn == level.CatOneCn &&
               CatOneEn == level.CatOneEn &&
               CatOneJp == level.CatOneJp &&
               CatOneKo == level.CatOneKo &&
               CatTwoCn == level.CatTwoCn &&
               CatTwoEn == level.CatTwoEn &&
               CatTwoJp == level.CatTwoJp &&
               CatTwoKo == level.CatTwoKo &&
               CatThreeCn == level.CatThreeCn &&
               CatThreeEn == level.CatThreeEn &&
               CatThreeJp == level.CatThreeJp &&
               CatThreeKo == level.CatThreeKo &&
               LevelId == level.LevelId &&
               Width == level.Width &&
               Height == level.Height;
    }
}
