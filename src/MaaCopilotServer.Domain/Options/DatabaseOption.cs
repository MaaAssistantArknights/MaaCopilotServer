// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The <c>Database</c> option.
/// </summary>
[OptionName("Database")]
[ExcludeFromCodeCoverage]
public class DatabaseOption
{
    /// <summary>
    ///     The database host.
    /// </summary>
    [JsonPropertyName("Host")]
    public string Host { get; set; } = null!;
    
    /// <summary>
    ///     The database port.
    /// </summary>
    [JsonPropertyName("Port")]
    public int Port { get; set; }
    
    /// <summary>
    ///     The database name.
    /// </summary>
    [JsonPropertyName("Database")]
    public string Database { get; set; } = null!;
    
    /// <summary>
    ///     The database user.
    /// </summary>
    [JsonPropertyName("Username")]
    public string Username { get; set; } = null!;
    
    /// <summary>
    ///     The database password.
    /// </summary>
    [JsonPropertyName("Password")]
    public string Password { get; set; } = null!;
}
