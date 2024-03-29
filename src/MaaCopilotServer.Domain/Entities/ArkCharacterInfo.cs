// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.GameData.Entity;

namespace MaaCopilotServer.Domain.Entities;

/// <summary>
///     Ark character info.
/// </summary>
public class ArkCharacterInfo : BaseEntity
{
#pragma warning disable CS8618
    public ArkCharacterInfo(ArkCharacterInfoGlobal ch)
    {
        Update(ch);
        CreateBy = Guid.Empty;
    }
#pragma warning restore CS8618

#pragma warning disable CS8618
    // ReSharper disable once UnusedMember.Local
    private ArkCharacterInfo() { }
#pragma warning restore CS8618

    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

    public ArkI18N Name { get; private set; } = new();

    public string Id { get; private set; }
    public string Profession { get; private set; }
    public int Star { get; private set; }

    public void Update(ArkCharacterInfoGlobal ch)
    {
        Name.Update(ch.Name);

        Id = ch.Id;
        Profession = ch.Profession;
        Star = ch.Star;
    }

    public bool IsEqual(ArkCharacterInfoGlobal ch)
    {
        return Name.IsEqual(ch.Name) &&
               Id == ch.Id &&
               Profession == ch.Profession &&
               Star == ch.Star;
    }
}
