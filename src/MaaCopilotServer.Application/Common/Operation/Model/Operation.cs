// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Operation.Model;

public record Operation
{
    [JsonPropertyName("stage_name")]
    public string? StageName { get; set; }
    [JsonPropertyName("minimum_required")]
    public string? MinimumRequired { get; set; }
    [JsonPropertyName("doc")]
    public Doc? Doc { get; set; }
    [JsonPropertyName("opers")]
    public Operator[]? Operators { get; set; }
    [JsonPropertyName("groups")]
    public Group[]? Groups { get; set; }
    [JsonPropertyName("actions")]
    public Action[]? Actions { get; set; }
}
