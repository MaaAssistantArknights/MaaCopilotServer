// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.Application.Common.Extensions;

public static class OperatorProfessionExtension
{
    public static string GetCharacterProfessionCnString(this string profession) => profession.ToLower() switch
    {
        "medic" => "医疗",
        "special" => "特种",
        "warrior" => "近卫",
        "sniper" => "狙击",
        "tank" => "重装",
        "caster" => "术士",
        "pioneer" => "先锋",
        "support" => "辅助",
        "token" => "非干员实体",
        "trap" => "场景实体",
        _ => "未知",
    };

    public static string GetCharacterProfessionEnString(this string profession) => profession.ToLower() switch
    {
        "medic" => "Medic",
        "special" => "Specialist",
        "warrior" => "Guard",
        "sniper" => "Sniper",
        "tank" => "Defender",
        "caster" => "Caster",
        "pioneer" => "Vanguard",
        "support" => "Supporter",
        "token" => "Non-Operator Entity",
        "trap" => "Environment Entity",
        _ => "Unknown",
    };

    public static string GetCharacterProfessionJpString(this string profession) => profession.ToLower() switch
    {
        "medic" => "医療",
        "special" => "特殊",
        "warrior" => "前衛",
        "sniper" => "狙撃",
        "tank" => "重装",
        "caster" => "術師",
        "pioneer" => "先鋒",
        "support" => "補助",
        "token" => "非オペレーターエンティティ",
        "trap" => "シーンエンティティ",
        _ => "不明",
    };

    public static string GetCharacterProfessionKoString(this string profession) => profession.ToLower() switch
    {
        "medic" => "메딕",
        "special" => "스페셜리스트",
        "warrior" => "가드",
        "sniper" => "스나이퍼",
        "tank" => "디펜더",
        "caster" => "캐스터",
        "pioneer" => "뱅가드",
        "support" => "서포터",
        "token" => "비 운영자 엔터티",
        "trap" => "장면 개체",
        _ => "알려지지 않은",
    };
}
