// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.GameData.Model;

public record ArkGameData
{
    public List<ArkActivity> ArkActivities { get; set; } = new();
    public List<ArkCharacter> ArkCharacters { get; set; } = new();
    public List<ArkLevel> ArkLevels { get; set; } = new();
    public List<ArkStage> ArkStages { get; set; } = new();
    public List<ArkZone> ArkZones { get; set; } = new();
    public Dictionary<string, string> ArkZoneActMap { get; set; } = new();
}
