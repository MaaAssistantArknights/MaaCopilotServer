// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The <c>Jwt</c> option.
/// </summary>
[OptionName("Jwt")]
[ExcludeFromCodeCoverage]
public class JwtOption
{
    /// <summary>
    ///     The secret.
    /// </summary>
    [JsonPropertyName("Secret")]
    public string Secret { get; set; } = null!;

    /// <summary>
    ///     The issuer.
    /// </summary>
    [JsonPropertyName("Issuer")]
    public string Issuer { get; set; } = null!;

    /// <summary>
    ///     The audience.
    /// </summary>
    [JsonPropertyName("Audience")]
    public string Audience { get; set; } = null!;

    /// <summary>
    ///     The time of expiration.
    /// </summary>
    [JsonPropertyName("ExpireTime")]
    public int ExpireTime { get; set; }
}
