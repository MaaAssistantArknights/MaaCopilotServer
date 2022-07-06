// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Operation.Model;

public record Doc
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    [JsonPropertyName("details")]
    public string? Details { get; set; }
    [JsonPropertyName("title_color")]
    public string? TitleColor { get; set; }
    [JsonPropertyName("details_color")]
    public string? DetailsColor { get; set; }
}
