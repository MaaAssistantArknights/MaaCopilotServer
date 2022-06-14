// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

[OptionName("Switches")]
public class SwitchesOption
{
    [JsonPropertyName("ElasticSearch")] public bool ElasticSearch { get; set; }
    [JsonPropertyName("Apm")] public bool Apm { get; set; }
}
