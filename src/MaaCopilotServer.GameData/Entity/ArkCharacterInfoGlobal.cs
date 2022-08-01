// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.GameData.Model;

namespace MaaCopilotServer.GameData.Entity;

public class ArkCharacterInfoGlobal
{
    internal ArkCharacterInfoGlobal(ArkCharacter cn, ArkCharacter? cnT, ArkCharacter? en, ArkCharacter? jp, ArkCharacter? ko)
    {
        Name = new ArkI18N(cn.Name, cnT?.Name, en?.Name, jp?.Name, ko?.Name);
        
        Id = cn.Id;
        Profession = cn.Profession;
        Star = cn.Rarity + 1;
    }

    public ArkI18N Name { get; }
    public string Id { get; }
    public string Profession { get; }
    public int Star { get; }
}
