// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Operation.Model;

public class Operator
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("skill")]
    public int? Skill { get; set; }
    [JsonPropertyName("skill_usage")]
    public int? SkillUsage { get; set; }
    [JsonPropertyName("requirements")]
    public OperatorRequirement? Requirements { get; set; } = null;
}
