// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.GameData.Model;

/// <summary>
///     Data from <c>level.json</c>
/// </summary>
public record ArkLevel
{
    /// <summary>
    ///     Level name
    /// </summary>
    /// <example>
    ///     坍塌
    /// </example>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Stage code
    /// </summary>
    /// <example>
    ///     0-1
    /// </example>
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    ///     Level id
    /// </summary>
    /// <example>
    ///     obt/main/level_main_00-01
    /// </example>
    [JsonPropertyName("levelId")]
    public string LevelId { get; set; } = string.Empty;

    /// <summary>
    ///     Stage id
    /// </summary>
    /// <example>
    ///     main_00-01
    /// </example>
    [JsonPropertyName("stageId")]
    public string StageId { get; set; } = string.Empty;

    /// <summary>
    ///     Level map width
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; set; }

    /// <summary>
    ///     Level map height
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; set; }
}
