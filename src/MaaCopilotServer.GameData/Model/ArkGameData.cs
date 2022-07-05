// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

namespace MaaCopilotServer.GameData.Model;

internal record ArkGameData
{
    internal List<ArkActivity> ArkActivities { get; set; } = new();
    internal List<ArkCharacter> ArkCharacters { get; set; } = new();
    internal List<ArkLevel> ArkLevels { get; set; } = new();
    internal List<ArkStage> ArkStages { get; set; } = new();
    internal List<ArkZone> ArkZones { get; set; } = new();
    internal Dictionary<string, string> ArkZoneActMap { get; set; } = new();
}
