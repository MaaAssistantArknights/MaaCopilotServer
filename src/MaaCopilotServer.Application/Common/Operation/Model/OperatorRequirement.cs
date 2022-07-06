// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Operation.Model;

public record OperatorRequirement
{
    [JsonPropertyName("elite")]
    public int? Elite { get; set; }
    [JsonPropertyName("level")]
    public int? Level { get; set; }
    [JsonPropertyName("skill_level")]
    public int? SkillLevel { get; set; }
    [JsonPropertyName("module")]
    public int? Module { get; set; }
    [JsonPropertyName("potentiality")]
    public int? Potentiality { get; set; }
}
