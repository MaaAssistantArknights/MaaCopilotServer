using System.Text.Json;
using MaaCopilotServer.GameData.Constants;
using MaaCopilotServer.GameData.Entity;
using MaaCopilotServer.GameData.Exceptions;
using MaaCopilotServer.GameData.Model;

namespace MaaCopilotServer.GameData;

/*

About level categories:

1. MAINLINE

    Main story level will be tagged like this:
        
        MAINLINE -> CHAPTER_NAME -> StageCode == obt/main/LEVEL_ID
        
    eg:
        
        主题曲 -> 序章：黑暗时代·上 -> 0-1 == obt/main/level_main_00-01
        主题曲 -> 第四章：急性衰竭 -> S4-7 == obt/main/level_sub_04-3-1

2. WEEKLY

    Weekly level will be tagged like this:
        
        WEEKLY -> WEEKLY_ZONE_NAME -> StageCode == obt/weekly/LEVEL_ID

    eg:
    
        资源收集 -> 空中威胁 -> CA-5 == obt/weekly/level_weekly_fly_5
        资源收集 -> 身先士卒 -> PR-D-2 == obt/promote/level_promote_d_2

3. ACTIVITY

    Activity level will be tagged like this:
    
        Activity -> ACT_NAME -> StageCode == activities/ACT_ID/LEVEL_ID

    eg:
        
        活动关卡 -> 战地秘闻 -> SW-EV-1 == activities/act4d0/level_act4d0_01

4. CAMPAIGN

    Campaign level will be tagged like this:
        
        CAMPAIGN -> CAMPAIGN_CODE -> CAMPAIGN_NAME == obt/campaign/LEVEL_ID

    eg:
    
        剿灭作战	-> 炎国 -> 龙门外环 == obt/campaign/level_camp_02

5. MEMORY

    Memory level will be tagged like this:
        
        MEMORY -> POSITION -> OPERATOR_NAME == obt/memory/LEVEL_ID
    
    eg:
    
        悖论模拟 -> 狙击 -> 克洛丝 == obt/memory/level_memory_kroos_1

*/

public static class GameDataParser
{
    /// <summary>
    ///     Parse ArkData JSON string to <see cref="ArkLevelEntityGlobal"/>
    /// </summary>
    /// <param name="cn">ArkData CN server.</param>
    /// <param name="en">ArkData Global server.</param>
    /// <param name="jp">ArkData Japan server.</param>
    /// <param name="ko">ArkData Korea server.</param>
    /// <param name="loggerCallback">A logger to log exceptions.</param>
    /// <returns></returns>
    public static ArkDataParsed Parse(ArkDataSource cn, ArkDataSource en, ArkDataSource jp, ArkDataSource ko, Action<Exception>? loggerCallback)
    {
        var cnGd = cn.ParseToGameDataModel();
        var enGd = en.ParseToGameDataModel();
        var jpGd = jp.ParseToGameDataModel();
        var koGd = ko.ParseToGameDataModel();

        var cnEs = cnGd.ParseToEntitySingleLanguage(cn.Language, loggerCallback).ToList();
        var enEs = enGd.ParseToEntitySingleLanguage(en.Language, loggerCallback).ToList();
        var jpEs = jpGd.ParseToEntitySingleLanguage(jp.Language, loggerCallback).ToList();
        var koEs = koGd.ParseToEntitySingleLanguage(ko.Language, loggerCallback).ToList();

        var globalEs = (
                from cnE in cnEs
                let enE = enEs.FirstOrDefault(x => x.LevelId == cnE.LevelId)
                let jpE = jpEs.FirstOrDefault(x => x.LevelId == cnE.LevelId)
                let koE = koEs.FirstOrDefault(x => x.LevelId == cnE.LevelId)
                select new ArkLevelEntityGlobal(cnE, enE, jpE, koE)
            ).ToList();

        var globalChar = (
                from cnC in cnGd.ArkCharacters
                let enC = enGd.ArkCharacters.FirstOrDefault(x => x.Id == cnC.Id)
                let jpC = jpGd.ArkCharacters.FirstOrDefault(x => x.Id == cnC.Id)
                let koC = koGd.ArkCharacters.FirstOrDefault(x => x.Id == cnC.Id)
                select new ArkCharacterInfoGlobal(cnC, enC, jpC, koC)
            ).ToList();

        return new ArkDataParsed
        {
            ArkCharacterInfos = globalChar,
            ArkLevelEntities = globalEs
        };
    }

    private static IEnumerable<ArkLevelEntity> ParseToEntitySingleLanguage(this ArkGameData gd, ArkServerLanguage language, Action<Exception>? loggerCallback)
    {
        var es = new List<ArkLevelEntity>();

        var levels = gd.ArkLevels.DistinctBy(x => x.LevelId).ToList();

        var levelMainLine = levels.Where(x => x.LevelId.ToLower().StartsWith("obt/main")).ToList();
        var levelWeekly = levels.Where(x => x.LevelId.ToLower().StartsWith("obt/weekly") || x.LevelId.ToLower().StartsWith("obt/promote")).ToList();
        var levelActivity = levels.Where(x => x.LevelId.ToLower().StartsWith("activities")).ToList();
        var levelCampaign = levels.Where(x => x.LevelId.ToLower().StartsWith("obt/campaign")).ToList();
        var levelMemory = levels.Where(x => x.LevelId.ToLower().StartsWith("obt/memory")).ToList();

        // MainLine
        foreach (var level in levelMainLine)
        {
            try
            {
                var chapterLevelId = level.LevelId.Split("/")[2];                  // level_main_10-02
                var chapterStrSplit = chapterLevelId.Split("_");                  // level main 10-02
                var diff = GetMainLineLevelDifficulty(chapterStrSplit[1]);    // main
                var stageCodeEncoded = chapterStrSplit.Last();                              // 10-02  remark: obt/main/level_easy_sub_09-1-1
                var chapterStr = stageCodeEncoded.Split("-")[0];                   // 10 (str)
                var chapter = int.Parse(chapterStr);                                          // 10 (int)

                var catThreeEx = "";
                if (chapter >= 9)
                {
                    catThreeEx = $" ({diff.GetMainLineLevelDifficultyI18NString(language)})";
                }


                var stage = gd.ArkStages
                    .Where(x => string.Equals(x.LevelId, level.LevelId, StringComparison.CurrentCultureIgnoreCase))
                    .Where(x => x.Code == level.Code)
                    .FirstOrDefault(x => x.StageId == level.StageId);

                if (stage is null)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "stage", level.ToString());
                }

                var zone = gd.ArkZones.FirstOrDefault(x => x.ZoneId == stage.ZoneId);

                if (zone is null)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "zone", level.ToString());
                }

                es.Add(new ArkLevelEntity(
                    ZoneTypes.MainLine.GetZoneTypeI18NString(language),
                    $"{zone.ZoneNameFirst} {zone.ZoneNameSecond}",
                    $"{level.Code}{catThreeEx}",
                    level));
            }
            catch (GameDataParseException e)
            {
                loggerCallback?.Invoke(e);
            }
            catch (Exception e)
            {
                loggerCallback?.Invoke(e);
                throw;
            }
        }

        // Weekly
        foreach (var level in levelWeekly)
        {
            try
            {
                var stage = gd.ArkStages
                    .Where(x => string.Equals(x.LevelId, level.LevelId, StringComparison.CurrentCultureIgnoreCase))
                    .Where(x => x.Code == level.Code)
                    .FirstOrDefault(x => x.StageId == level.StageId);

                if (stage is null)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "stage", level.ToString());
                }

                var zone = gd.ArkZones.FirstOrDefault(x => x.ZoneId == stage.ZoneId);

                if (zone is null)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "zone", level.ToString());
                }

                es.Add(new ArkLevelEntity(
                    ZoneTypes.Weekly.GetZoneTypeI18NString(language),
                    zone.ZoneNameSecond,
                    level.Code,
                    level));
            }
            catch (GameDataParseException e)
            {
                loggerCallback?.Invoke(e);
            }
            catch (Exception e)
            {
                loggerCallback?.Invoke(e);
                throw;
            }
        }

        // Activity
        foreach (var level in levelActivity)
        {
            try
            {
                var stage = gd.ArkStages
                    .Where(x => string.Equals(x.LevelId, level.LevelId, StringComparison.CurrentCultureIgnoreCase))
                    .Where(x => x.Code == level.Code)
                    .FirstOrDefault(x => x.StageId == level.StageId);

                if (stage is null)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "stage", level.ToString());
                }

                var hasId = gd.ArkZoneActMap.ContainsKey(stage.ZoneId);

                if (hasId is false)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "zone_act_map", level.ToString());
                }

                var actId = gd.ArkZoneActMap[stage.ZoneId];

                var act = gd.ArkActivities.FirstOrDefault(x => x.Id == actId);

                if (act is null)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "activity", level.ToString());
                }

                es.Add(new ArkLevelEntity(
                    ZoneTypes.Activity.GetZoneTypeI18NString(language),
                    act.Name,
                    level.Code,
                    level));
            }
            catch (GameDataParseException e)
            {
                loggerCallback?.Invoke(e);
            }
            catch (Exception e)
            {
                loggerCallback?.Invoke(e);
                throw;
            }
        }

        // Campaign
        es.AddRange(from level in levelCampaign
            select new ArkLevelEntity(
                ZoneTypes.Campaign.GetZoneTypeI18NString(language),
                level.Code,
                level.Name,
                level));

        // Memory
        foreach (var level in levelMemory)
        {
            try
            {
                var chIdSplit = level.StageId.Split("_");

                if (chIdSplit.Length != 3)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), "ch_id", level.ToString());
                }

                var chId = chIdSplit[1];

                var ch = gd.ArkCharacters.FirstOrDefault(x => x.Id.EndsWith(chId));

                if (ch is null)
                {
                    throw new GameDataParseException("MainLine", language.ToString(), $"ch_{chId}", level.ToString());
                }

                es.Add(new ArkLevelEntity(
                    ZoneTypes.Memory.GetZoneTypeI18NString(language),
                    GetCharacterProfessionType(ch.Profession).GetCharacterProfessionI18NString(language),
                    ch.Name,
                    level));
            }
            catch (GameDataParseException e)
            {
                loggerCallback?.Invoke(e);
            }
            catch (Exception e)
            {
                loggerCallback?.Invoke(e);
                throw;
            }
        }

        return es;
    }

    private static ArkGameData ParseToGameDataModel(this ArkDataSource dataSource)
    {
        var arkActDoc = JsonDocument.Parse(dataSource.ArkAct).RootElement;
        var arkActStr = arkActDoc.GetProperty("basicInfo").ToString();
        var arkZoneActMapStr = arkActDoc.GetProperty("zoneToActivity").ToString();
        var arkStageStr = JsonDocument.Parse(dataSource.ArkStage).RootElement.GetProperty("stages").ToString();
        var arkZoneStr = JsonDocument.Parse(dataSource.ArkZone).RootElement.GetProperty("zones").ToString();

        var arkActObj = JsonSerializer.Deserialize<Dictionary<string, ArkActivity>>(arkActStr)?.Values.ToList() ?? new List<ArkActivity>();
        var arkCharObj = JsonSerializer.Deserialize<Dictionary<string, ArkCharacter>>(dataSource.ArkChar)?
            .Select(x => new ArkCharacter { Id = x.Key, Name = x.Value.Name, Profession = x.Value.Profession, Rarity = x.Value.Rarity })
            .ToList() ?? new List<ArkCharacter>();
        var arkLevelObj = JsonSerializer.Deserialize<List<ArkLevel>>(dataSource.ArkLevel) ?? new List<ArkLevel>();
        var arkStageObj = JsonSerializer.Deserialize<Dictionary<string, ArkStage>>(arkStageStr)?.Values.ToList() ?? new List<ArkStage>();
        var arkZoneObj = JsonSerializer.Deserialize<Dictionary<string, ArkZone>>(arkZoneStr)?.Values.ToList() ?? new List<ArkZone>();
        var arkZoneActMapObj = JsonSerializer.Deserialize<Dictionary<string, string>>(arkZoneActMapStr) ?? new Dictionary<string, string>();

        return new ArkGameData
        {
            ArkActivities = arkActObj,
            ArkCharacters = arkCharObj,
            ArkLevels = arkLevelObj,
            ArkStages = arkStageObj,
            ArkZones = arkZoneObj,
            ArkZoneActMap = arkZoneActMapObj
        };
    }

    private static CharacterProfessions GetCharacterProfessionType(string pro) => pro.ToLower() switch
    {
        "medic" => CharacterProfessions.Medic,
        "special" => CharacterProfessions.Special,
        "warrior" => CharacterProfessions.Warrior,
        "sniper" => CharacterProfessions.Sniper,
        "tank" => CharacterProfessions.Tank,
        "caster" => CharacterProfessions.Caster,
        "pioneer" => CharacterProfessions.Pioneer,
        "support" => CharacterProfessions.Support,
        _ => CharacterProfessions.Unknown
    };

    private static MainLineLevelDifficulty GetMainLineLevelDifficulty(string diff) => diff.ToLower() switch
    {
        "easy" => MainLineLevelDifficulty.Easy,
        "main" => MainLineLevelDifficulty.Main,
        "tough" => MainLineLevelDifficulty.Tough,
        _ => MainLineLevelDifficulty.Unknown
    };
}
