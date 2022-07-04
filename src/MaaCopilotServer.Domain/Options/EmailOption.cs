// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     Email sender options.
/// </summary>
[OptionName("Email")]
[ExcludeFromCodeCoverage]
public class EmailOption
{
    /// <summary>
    ///     The sender options
    /// </summary>
    [JsonPropertyName("Sender")] public EmailSenderOption Sender { get; set; } = null!;
    /// <summary>
    ///     The SMTP server options.
    /// </summary>
    [JsonPropertyName("Smtp")] public EmailSmtpOption Smtp { get; set; } = null!;
}

/// <summary>
///     Email sender options.
/// </summary>
[ExcludeFromCodeCoverage]
public class EmailSenderOption
{
    /// <summary>
    ///     The sender's email address.
    /// </summary>
    [JsonPropertyName("Address")] public string Address { get; set; } = null!;
    /// <summary>
    ///     The sender's name.
    /// </summary>
    [JsonPropertyName("Name")] public string Name { get; set; } = null!;
}

/// <summary>
///     The email SMTP server option.
/// </summary>
[ExcludeFromCodeCoverage]
public class EmailSmtpOption
{
    /// <summary>
    ///     The SMTP server address.
    /// </summary>
    [JsonPropertyName("Host")] public string Host { get; set; } = null!;
    /// <summary>
    ///     The SMTP server port.
    /// </summary>
    [JsonPropertyName("Port")] public int Port { get; set; }
    /// <summary>
    ///     The SMTP server authentication username.
    /// </summary>
    [JsonPropertyName("Account")] public string Account { get; set; } = null!;
    /// <summary>
    ///     The SMTP server authentication password.
    /// </summary>
    [JsonPropertyName("Password")] public string Password { get; set; } = null!;
    /// <summary>
    ///     Whether to use SSL.
    /// </summary>
    [JsonPropertyName("UseSsl")] public bool UseSsl { get; set; }
    /// <summary>
    ///     Whether to use authentication.
    /// </summary>
    [JsonPropertyName("UseAuthentication")] public bool UseAuthentication { get; set; }
}
