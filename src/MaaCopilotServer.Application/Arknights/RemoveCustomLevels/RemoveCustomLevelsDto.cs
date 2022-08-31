// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Arknights.RemoveCustomLevels;

public record RemoveCustomLevelsDto
{
    /// <summary>
    ///     Total number of database changes.
    /// </summary>
    [JsonPropertyName("db_context_changes")]
    public int DbContextChanges { get; set; }

    /// <summary>
    ///     Removed levels.
    /// </summary>
    [JsonPropertyName("removed")]
    public List<string> Removed { get; set; } = new();
}
