// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
/// The <c>Database</c> option.
/// </summary>
[OptionName("Database")]
public class DatabaseOption
{
    /// <summary>
    /// The connection string.
    /// </summary>
    [JsonPropertyName("ConnectionString")] public string ConnectionString { get; set; } = null!;
}
