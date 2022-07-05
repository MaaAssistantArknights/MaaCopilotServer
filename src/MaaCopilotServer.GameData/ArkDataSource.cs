// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.GameData.Constants;

namespace MaaCopilotServer.GameData;

public struct ArkDataSource
{
    /// <summary>
    ///     Activity data from <c>activity_table.json</c>.
    /// </summary>
    public string ArkAct { get; set; }
    /// <summary>
    ///     Character data from <c>character_table.json</c>.
    /// </summary>
    public string ArkChar { get; set; }
    /// <summary>
    ///     Level data from <c>levels.json</c>.
    /// </summary>
    public string ArkLevel { get; set; }
    /// <summary>
    ///     Stage data from <c>stage_table.json</c>.
    /// </summary>
    public string ArkStage { get; set; }
    /// <summary>
    ///     Zone data from <c>zone_table.json</c>.
    /// </summary>
    public string ArkZone { get; set; }
    /// <summary>
    ///     Server region and language.
    /// </summary>
    public ArkServerLanguage Language { get; set; }
}
