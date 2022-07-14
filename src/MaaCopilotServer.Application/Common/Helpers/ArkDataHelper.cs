// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Arknights.GetLevelList;
using MaaCopilotServer.Application.Arknights.GetOperatorList;
using MaaCopilotServer.Domain.Entities;

namespace MaaCopilotServer.Application.Common.Helpers;

public static class ArkDataHelper
{
    public static Func<ArkLevelData, GetLevelListDto> GetLevelMapperFunc(this string? server) =>
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_levelMapCn, s_levelMapEn, s_levelMapJp, s_levelMapKo);

    public static Func<ArkCharacterInfo, GetOperatorListDto> GetCharMapperFunc(this string? server) =>
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_charMapCn, s_charMapEn, s_charMapJp, s_charMapKo);

    public static Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> GetLevelQueryFunc(this string? server) =>
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_levelQueryCn, s_levelQueryEn, s_levelQueryJp, s_levelQueryKo);

    public static Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> GetCharQueryFunc(this string? server) =>
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_charQueryCn, s_charQueryEn, s_charQueryJp, s_charQueryKo);

    #region Level Mapper

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapCn = data =>
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

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapEn = data =>
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

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapJp = data =>
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

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapKo = data =>
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

    #endregion

    #region Character Mapper

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapCn = data =>
        new GetOperatorListDto
        {
            Name = data.NameCn,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionCnString(),
            Star = data.Star
        };

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapEn = data =>
        new GetOperatorListDto
        {
            Name = data.NameEn,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionEnString(),
            Star = data.Star
        };

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapJp = data =>
        new GetOperatorListDto
        {
            Name = data.NameJp,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionJpString(),
            Star = data.Star
        };

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapKo = data =>
        new GetOperatorListDto
        {
            Name = data.NameKo,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionKoString(),
            Star = data.Star
        };

    #endregion

    #region Level IQueryable Mapper

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryCn = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.NameCn) == false &&
            string.IsNullOrEmpty(x.CatOneCn) == false &&
            string.IsNullOrEmpty(x.CatTwoCn) == false &&
            string.IsNullOrEmpty(x.CatThreeCn) == false);

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryEn = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.NameEn) == false &&
            string.IsNullOrEmpty(x.CatOneEn) == false &&
            string.IsNullOrEmpty(x.CatTwoEn) == false &&
            string.IsNullOrEmpty(x.CatThreeEn) == false);

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryJp = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.NameJp) == false &&
            string.IsNullOrEmpty(x.CatOneJp) == false &&
            string.IsNullOrEmpty(x.CatTwoJp) == false &&
            string.IsNullOrEmpty(x.CatThreeJp) == false);

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryKo = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.NameKo) == false &&
            string.IsNullOrEmpty(x.CatOneKo) == false &&
            string.IsNullOrEmpty(x.CatTwoKo) == false &&
            string.IsNullOrEmpty(x.CatThreeKo) == false);

    #endregion

    #region Character IQueryable Mapper

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryCn = q =>
        q.Where(x => string.IsNullOrEmpty(x.NameCn) == false);

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryEn = q =>
        q.Where(x => string.IsNullOrEmpty(x.NameEn) == false);

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryJp = q =>
        q.Where(x => string.IsNullOrEmpty(x.NameJp) == false);

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryKo = q =>
        q.Where(x => string.IsNullOrEmpty(x.NameKo) == false);

    #endregion
}
