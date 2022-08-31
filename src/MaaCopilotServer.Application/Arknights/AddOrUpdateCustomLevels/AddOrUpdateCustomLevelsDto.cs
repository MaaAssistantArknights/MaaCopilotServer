// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Arknights.AddOrUpdateCustomLevels;

public record AddOrUpdateCustomLevelsDto
{
    /// <summary>
    ///     Total number of database changes.
    /// </summary>
    [JsonPropertyName("db_context_changes")]
    public int DbContextChanges { get; set; }
    
    /// <summary>
    ///     Added levels.
    /// </summary>
    [JsonPropertyName("added")]
    public List<string> Added { get; set; } = new();
    
    /// <summary>
    ///     Updated levels.
    /// </summary>
    [JsonPropertyName("updated")]
    public List<string> Updated { get; set; } = new();
}
