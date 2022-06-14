// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

[OptionName("ElasticLogSink")]
public class ElasticLogSinkOption
{
    [JsonPropertyName("ApplicationName")] public string ApplicationName { get; set; } = null!;
    [JsonPropertyName("Uris")] public string Uris { get; set; } = null!;
    [JsonPropertyName("Period")] public int Period { get; set; }
    [JsonPropertyName("Authentication")] public ElasticLogSinkAuthenticationOption Authentication { get; set; } = null!;
}

public class ElasticLogSinkAuthenticationOption
{
    [JsonPropertyName("Method")] public string Method { get; set; } = null!;
    [JsonPropertyName("Secret")] public ElasticLogSinkAuthenticationSecretOption Secret { get; set; } = null!;
}

public class ElasticLogSinkAuthenticationSecretOption
{
    [JsonPropertyName("Id")] public string Id { get; set; } = null!;
    [JsonPropertyName("Key")] public string Key { get; set; } = null!;
}
