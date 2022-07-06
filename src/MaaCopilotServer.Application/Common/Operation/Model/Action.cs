// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Operation.Model;

public class Action
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("kills")]
    public int? Kills { get; set; }
    [JsonPropertyName("cost_changes")]
    public int? CostChanges { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("location")]
    public int[]? Location { get; set; }
    [JsonPropertyName("direction")]
    public string? Direction { get; set; }
    [JsonPropertyName("skill_usage")]
    public int? SkillUsage { get; set; }
    [JsonPropertyName("pre_delay")]
    public int? PreDelay { get; set; }
    [JsonPropertyName("rear_delay")]
    public int? RearDelay { get; set; }
    [JsonPropertyName("doc")]
    public string? Doc { get; set; }
    [JsonPropertyName("doc_color")]
    public string? DocColor { get; set; }
}
