// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.GameData.Model;

/// <summary>
///     Data from <c>activity_table.json</c>
/// </summary>
public record ArkActivity
{
    /// <summary>
    ///     Activity id
    /// </summary>
    /// <example>
    ///     act9d0
    /// </example>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Activity name
    /// </summary>
    /// <example>
    ///     生于黑夜
    /// </example>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
