// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Entities;

namespace MaaCopilotServer.Application.Common.Models;

public record ArkI18NDto
{
    [JsonPropertyName("cn")]
    public string ChineseSimplified { get; set; } = string.Empty;
    
    [JsonPropertyName("cn_tw")]
    public string ChineseTraditional { get; set; } = string.Empty;
    
    [JsonPropertyName("en")]
    public string English { get; set; } = string.Empty;
    
    [JsonPropertyName("jp")]
    public string Japanese { get; set; } = string.Empty;
    
    [JsonPropertyName("ko")]
    public string Korean { get; set; } = string.Empty;
}
