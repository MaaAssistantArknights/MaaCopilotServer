// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;

namespace MaaCopilotServer.Application.Common.Helpers;

public static class ArkI18NConvertor
{
    public static ArkI18N ToDomainEntityFromDto(this ArkI18NDto dto)
    {
        return new ArkI18N
        {
            ChineseSimplified = dto.ChineseSimplified,
            ChineseTraditional = dto.ChineseTraditional,
            English = dto.English,
            Japanese = dto.Japanese,
            Korean = dto.Korean
        };
    }
    
    public static GameData.Entity.ArkI18N ToGameDataEntityFromDto(this ArkI18NDto dto)
    {
        return new GameData.Entity.ArkI18N(dto.ChineseSimplified, dto.ChineseTraditional,
            dto.English, dto.Japanese, dto.Korean);
    }
    
    public static ArkI18N ToDomainEntityFromGameDataEntity(this GameData.Entity.ArkI18N dto)
    {
        return new ArkI18N
        {
            ChineseSimplified = dto.ChineseSimplified,
            ChineseTraditional = dto.ChineseTraditional,
            English = dto.English,
            Japanese = dto.Japanese,
            Korean = dto.Korean
        };
    }
    
    public static GameData.Entity.ArkI18N ToGameDataEntityFromDomainEntity(this ArkI18N dto)
    {
        return new GameData.Entity.ArkI18N(dto.ChineseSimplified, dto.ChineseTraditional,
            dto.English, dto.Japanese, dto.Korean);
    }
}
