// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The options for the server.
/// </summary>
[OptionName("CopilotServer")]
[ExcludeFromCodeCoverage]
public class CopilotServerOption
{
    /// <summary>
    ///     The default role for registered users. The default role will
    /// not above Uploader. Any permission level above Uploader will be
    /// reset to Uploader role.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("RegisterUserDefaultRole")]
    public UserRole RegisterUserDefaultRole { get; set; } = UserRole.User;

    /// <summary>
    ///     Enable the test email api at <c>/test/email?to={address}&amp;token={token}</c>
    /// endpoint. This is useful for testing purposes. But this should not be enabled in production.
    /// </summary>
    [JsonPropertyName("EnableTestEmailApi")]
    public bool EnableTestEmailApi { get; set; } = false;

    /// <summary>
    ///     The api token for the email test api. Add this token to the query string.
    /// </summary>
    [JsonPropertyName("TestEmailApiToken")]
    public string TestEmailApiToken { get; set; } = string.Empty;

    /// <summary>
    ///     GitHub api request use proxy or not.
    /// </summary>
    [JsonPropertyName("GitHubApiRequestProxyEnable")]
    public bool GitHubApiRequestProxyEnable { get; set; } = false;

    /// <summary>
    ///     GitHub api request proxy address.
    /// </summary>
    [JsonPropertyName("GitHubApiRequestProxyAddress")]
    public string GitHubApiRequestProxyAddress { get; set; } = string.Empty;

    /// <summary>
    ///     GitHub api request proxy port.
    /// </summary>
    [JsonPropertyName("GitHubApiRequestProxyPort")]
    public int GitHubApiRequestProxyPort { get; set; } = 0;

    /// <summary>
    ///     GitHub api request user agent.
    /// </summary>
    [JsonPropertyName("GitHubApiRequestUserAgent")]
    public string GitHubApiRequestUserAgent { get; set; } = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.66 Safari/537.36 Edg/103.0.1264.44";
}
