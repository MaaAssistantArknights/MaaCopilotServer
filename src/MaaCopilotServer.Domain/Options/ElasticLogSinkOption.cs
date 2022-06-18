// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The <c>ElasticLogSink</c> option.
/// </summary>
[OptionName("ElasticLogSink")]
public class ElasticLogSinkOption
{
    /// <summary>
    ///     The application name.
    /// </summary>
    [JsonPropertyName("ApplicationName")]
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    ///     The URI.
    /// </summary>
    [JsonPropertyName("Uris")]
    public string Uris { get; set; } = null!;

    /// <summary>
    ///     The period.
    /// </summary>
    [JsonPropertyName("Period")]
    public int Period { get; set; }

    /// <summary>
    ///     The authentication settings.
    /// </summary>
    [JsonPropertyName("Authentication")]
    public ElasticLogSinkAuthenticationOption Authentication { get; set; } = null!;
}

/// <summary>
///     The authentication settings of <c>ElasticLogSink</c>.
/// </summary>
public class ElasticLogSinkAuthenticationOption
{
    /// <summary>
    ///     The authentication method.
    /// </summary>
    [JsonPropertyName("Method")]
    public string Method { get; set; } = null!;

    /// <summary>
    ///     The secret.
    /// </summary>
    [JsonPropertyName("Secret")]
    public ElasticLogSinkAuthenticationSecretOption Secret { get; set; } = null!;
}

/// <summary>
///     The secret settings of authentication of <c>ElasticLogSink</c>.
/// </summary>
public class ElasticLogSinkAuthenticationSecretOption
{
    /// <summary>
    ///     The secret ID.
    /// </summary>
    [JsonPropertyName("Id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The secret key.
    /// </summary>
    [JsonPropertyName("Key")]
    public string Key { get; set; } = null!;
}
