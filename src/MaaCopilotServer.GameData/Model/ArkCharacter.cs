// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.GameData.Model;

/// <summary>
///     Data from <c>character_table.json</c>
/// </summary>
public record ArkCharacter
{
    /// <summary>
    ///     Character's name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Character's id.
    /// </summary>
    [JsonIgnore]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Character's profession.
    /// </summary>
    [JsonPropertyName("profession")]
    public string Profession { get; set; } = string.Empty;
}
