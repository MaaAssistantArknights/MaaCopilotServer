// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The <c>ElasticLogSink</c> option.
/// </summary>
[OptionName("ElasticLogSink")]
[ExcludeFromCodeCoverage]
public class ElasticLogSinkOption
{
    /// <summary>
    ///     The application name.
    /// </summary>
    [JsonPropertyName("ApplicationName")]
    public string ApplicationName { get; set; } = null!;

    /// <summary>
    ///     The ElasticSearch URI.
    /// </summary>
    [JsonPropertyName("Uris")]
    public string Uris { get; set; } = null!;

    /// <summary>
    ///     The message commit period.
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
[ExcludeFromCodeCoverage]
public class ElasticLogSinkAuthenticationOption
{
    /// <summary>
    ///     The authentication method, could be Basic or ApiKey.
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
[ExcludeFromCodeCoverage]
public class ElasticLogSinkAuthenticationSecretOption
{
    /// <summary>
    ///     The API Key ID or Username.
    /// </summary>
    [JsonPropertyName("Id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The API Key secret or Password.
    /// </summary>
    [JsonPropertyName("Key")]
    public string Key { get; set; } = null!;
}
