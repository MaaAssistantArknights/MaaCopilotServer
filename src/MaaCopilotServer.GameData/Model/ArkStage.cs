// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.GameData.Model;

/// <summary>
///     Data from <c>stage_table.json</c>
/// </summary>
internal record ArkStage
{
    /// <summary>
    ///     Level id, call <see cref="string.ToLower()"/> before use this value.
    /// </summary>
    /// <example>
    ///     Activities/ACT5D0/level_act5d0_ex08
    /// </example>
    [JsonPropertyName("levelId")]
    internal string LevelId { get; set; } = string.Empty;

    /// <summary>
    ///     Stage zone id
    /// </summary>
    /// <example>
    ///     act14d7_zone2
    /// </example>
    [JsonPropertyName("zoneId")]
    internal string ZoneId { get; set; } = string.Empty;

    /// <summary>
    ///     Stage id
    /// </summary>
    /// <example>
    ///     act5d0_ex08
    /// </example>
    [JsonPropertyName("stageId")]
    internal string StageId { get; set; } = string.Empty;

    /// <summary>
    ///     Stage code
    /// </summary>
    /// <example>
    ///    CB-EX8
    /// </example>
    [JsonPropertyName("code")]
    internal string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Stage name
    /// </summary>
    /// <example>
    ///     日常
    /// </example>
    [JsonPropertyName("name")]
    internal string Name { get; set; } = string.Empty;
}
