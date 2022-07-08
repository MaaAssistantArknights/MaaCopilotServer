// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The <c>Switches</c> option.
/// </summary>
[OptionName("Switches")]
[ExcludeFromCodeCoverage]
public class SwitchesOption
{
    /// <summary>
    ///     Indicates whether Elastic Search feature is switched on/off.
    /// </summary>
    [JsonPropertyName("ElasticSearch")]
    public bool ElasticSearch { get; set; }

    /// <summary>
    ///     Indicates whether APM feature is switched on/off.
    /// </summary>
    [JsonPropertyName("Apm")]
    public bool Apm { get; set; }
}
