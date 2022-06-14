// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

[OptionName("Jwt")]
public class JwtOption
{
    [JsonPropertyName("Secret")] public string Secret { get; set; } = null!;
    [JsonPropertyName("Issuer")] public string Issuer { get; set; } = null!;
    [JsonPropertyName("Audience")] public string Audience { get; set; } = null!;
    [JsonPropertyName("ExpireTime")] public int ExpireTime { get; set; }
}
