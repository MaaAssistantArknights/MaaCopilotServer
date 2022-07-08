// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Operation.Model;

public record Group
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("opers")]
    public Operator[]? Operators { get; set; }
}
