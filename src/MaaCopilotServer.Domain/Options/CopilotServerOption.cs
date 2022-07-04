// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The options for the server.
/// </summary>
[OptionName("CopilotServer")]
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
}
