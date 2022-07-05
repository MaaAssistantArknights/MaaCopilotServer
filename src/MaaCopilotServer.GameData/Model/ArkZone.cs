// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.GameData.Model;

/// <summary>
///     Data from <c>zone_table.json</c>
/// </summary>
internal record ArkZone
{
    /// <summary>
    ///     Zone id
    /// </summary>
    /// <example>
    ///     main_10
    /// </example>
    [JsonPropertyName("zoneID")]
    internal string ZoneId { get; set; } = string.Empty;

    /// <summary>
    ///     Zone name first
    /// </summary>
    /// <example>
    ///     第十章
    /// </example>
    [JsonPropertyName("zoneNameFirst")]
    internal string ZoneNameFirst { get; set; } = string.Empty;

    /// <summary>
    ///     Zone name second
    /// </summary>
    /// <example>
    ///     破碎日冕
    /// </example>
    [JsonPropertyName("zoneNameSecond")]
    internal string ZoneNameSecond { get; set; } = string.Empty;
}
