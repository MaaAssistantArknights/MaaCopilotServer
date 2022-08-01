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
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_levelMapCn, s_levelMapTw, s_levelMapEn, s_levelMapJp, s_levelMapKo);

    public static Func<ArkCharacterInfo, GetOperatorListDto> GetCharMapperFunc(this string? server) =>
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_charMapCn, s_charMapTw, s_charMapEn, s_charMapJp, s_charMapKo);

    public static Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> GetLevelQueryFunc(this string? server) =>
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_levelQueryCn, s_levelQueryTw, s_levelQueryEn, s_levelQueryJp, s_levelQueryKo);

    public static Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> GetCharQueryFunc(this string? server) =>
        ArkServerLanguage.Parse(server).GetArkServerLanguageSpecificAction(s_charQueryCn, s_charQueryTw, s_charQueryEn, s_charQueryJp, s_charQueryKo);

    #region Level Mapper

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapCn = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOne.ChineseSimplified,
                CatTwo = data.CatTwo.ChineseSimplified,
                CatThree = data.CatThree.ChineseSimplified,
                Name = data.Name.ChineseSimplified,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };
    
    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapTw = data =>
        new GetLevelListDto
        {
            CatOne = data.CatOne.ChineseTraditional,
            CatTwo = data.CatTwo.ChineseTraditional,
            CatThree = data.CatThree.ChineseTraditional,
            Name = data.Name.ChineseTraditional,
            LevelId = data.LevelId,
            Height = data.Height,
            Width = data.Width
        };

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapEn = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOne.English,
                CatTwo = data.CatTwo.English,
                CatThree = data.CatThree.English,
                Name = data.Name.English,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapJp = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOne.Japanese,
                CatTwo = data.CatTwo.Japanese,
                CatThree = data.CatThree.Japanese,
                Name = data.Name.Japanese,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };

    private static readonly Func<ArkLevelData, GetLevelListDto> s_levelMapKo = data =>
            new GetLevelListDto
            {
                CatOne = data.CatOne.Korean,
                CatTwo = data.CatTwo.Korean,
                CatThree = data.CatThree.Korean,
                Name = data.Name.Korean,
                LevelId = data.LevelId,
                Height = data.Height,
                Width = data.Width
            };

    #endregion

    #region Character Mapper

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapCn = data =>
        new GetOperatorListDto
        {
            Name = data.Name.ChineseSimplified,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionCnString(),
            Star = data.Star
        };
    
    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapTw = data =>
        new GetOperatorListDto
        {
            Name = data.Name.ChineseTraditional,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionCnString(),
            Star = data.Star
        };

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapEn = data =>
        new GetOperatorListDto
        {
            Name = data.Name.English,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionEnString(),
            Star = data.Star
        };

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapJp = data =>
        new GetOperatorListDto
        {
            Name = data.Name.Japanese,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionJpString(),
            Star = data.Star
        };

    private static readonly Func<ArkCharacterInfo, GetOperatorListDto> s_charMapKo = data =>
        new GetOperatorListDto
        {
            Name = data.Name.Korean,
            Id = data.Id,
            Profession = data.Profession.GetCharacterProfessionKoString(),
            Star = data.Star
        };

    #endregion

    #region Level IQueryable Mapper

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryCn = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.Name.ChineseSimplified) == false &&
            string.IsNullOrEmpty(x.CatOne.ChineseSimplified) == false &&
            string.IsNullOrEmpty(x.CatTwo.ChineseSimplified) == false &&
            string.IsNullOrEmpty(x.CatThree.ChineseSimplified) == false);
    
    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryTw = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.Name.ChineseTraditional) == false &&
            string.IsNullOrEmpty(x.CatOne.ChineseTraditional) == false &&
            string.IsNullOrEmpty(x.CatTwo.ChineseTraditional) == false &&
            string.IsNullOrEmpty(x.CatThree.ChineseTraditional) == false);

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryEn = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.Name.English) == false &&
            string.IsNullOrEmpty(x.CatOne.English) == false &&
            string.IsNullOrEmpty(x.CatTwo.English) == false &&
            string.IsNullOrEmpty(x.CatThree.English) == false);

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryJp = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.Name.Japanese) == false &&
            string.IsNullOrEmpty(x.CatOne.Japanese) == false &&
            string.IsNullOrEmpty(x.CatTwo.Japanese) == false &&
            string.IsNullOrEmpty(x.CatThree.Japanese) == false);

    private static readonly Func<IQueryable<ArkLevelData>, IQueryable<ArkLevelData>> s_levelQueryKo = q =>
        q.Where(x =>
            string.IsNullOrEmpty(x.Name.Korean) == false &&
            string.IsNullOrEmpty(x.CatOne.Korean) == false &&
            string.IsNullOrEmpty(x.CatTwo.Korean) == false &&
            string.IsNullOrEmpty(x.CatThree.Korean) == false);

    #endregion

    #region Character IQueryable Mapper

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryCn = q =>
        q.Where(x => string.IsNullOrEmpty(x.Name.ChineseSimplified) == false);

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryTw = q =>
        q.Where(x => string.IsNullOrEmpty(x.Name.ChineseTraditional) == false);
    
    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryEn = q =>
        q.Where(x => string.IsNullOrEmpty(x.Name.English) == false);

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryJp = q =>
        q.Where(x => string.IsNullOrEmpty(x.Name.Japanese) == false);

    private static readonly Func<IQueryable<ArkCharacterInfo>, IQueryable<ArkCharacterInfo>> s_charQueryKo = q =>
        q.Where(x => string.IsNullOrEmpty(x.Name.Korean) == false);

    #endregion

    #region Level Query in Operation

    public static Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>
        GetQueryLevelNameFunc(this string? server) => ArkServerLanguage.Parse(server)
        .GetArkServerLanguageSpecificAction
        <Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>>(
            (q, s) => q.Where(x => x.ArkLevel.Name.ChineseSimplified.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.Name.ChineseTraditional.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.Name.English.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.Name.Japanese.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.Name.Korean.Contains(s))
        );

    public static Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>
        GetQueryLevelCatOneFunc(this string? server) => ArkServerLanguage.Parse(server)
        .GetArkServerLanguageSpecificAction
            <Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>>(
            (q, s) => q.Where(x => x.ArkLevel.CatOne.ChineseSimplified.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatOne.ChineseTraditional.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatOne.English.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatOne.Japanese.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatOne.Korean.Contains(s))
            );

    public static Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>
        GetQueryLevelCatTwoFunc(this string? server) => ArkServerLanguage.Parse(server)
        .GetArkServerLanguageSpecificAction
            <Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>>(
            (q, s) => q.Where(x => x.ArkLevel.CatTwo.ChineseSimplified.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatTwo.ChineseTraditional.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatTwo.English.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatTwo.Japanese.Contains(s)),
            (q, s) => q.Where(x => x.ArkLevel.CatTwo.Korean.Contains(s))
            );

    public static Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>
        GetQueryLevelCatThreeFunc(this string? server) => ArkServerLanguage.Parse(server)
        .GetArkServerLanguageSpecificAction
            <Func<IQueryable<Domain.Entities.CopilotOperation>, string, IQueryable<Domain.Entities.CopilotOperation>>>(
                (q, s) => q.Where(x => x.ArkLevel.CatThree.ChineseSimplified.Contains(s)),
                (q, s) => q.Where(x => x.ArkLevel.CatThree.ChineseTraditional.Contains(s)),
                (q, s) => q.Where(x => x.ArkLevel.CatThree.English.Contains(s)),
                (q, s) => q.Where(x => x.ArkLevel.CatThree.Japanese.Contains(s)),
                (q, s) => q.Where(x => x.ArkLevel.CatThree.Korean.Contains(s))
            );

    #endregion
}
