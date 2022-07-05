// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.GameData.Model;

namespace MaaCopilotServer.GameData.Entity;

public class ArkCharacterInfoGlobal
{
    internal ArkCharacterInfoGlobal(ArkCharacter cn, ArkCharacter? en, ArkCharacter? jp, ArkCharacter? ko)
    {
        NameCn = cn.Name;
        NameEn = en?.Name ?? string.Empty;
        NameJp = jp?.Name ?? string.Empty;
        NameKo = ko?.Name ?? string.Empty;

        Id = cn.Id;
        Profession = cn.Profession;
        Star = cn.Rarity + 1;
    }

    public string NameCn { get; }
    public string NameEn { get; }
    public string NameJp { get; }
    public string NameKo { get; }

    public string Id { get; }
    public string Profession { get; }
    public int Star { get; }
}
