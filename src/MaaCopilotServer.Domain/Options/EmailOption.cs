// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

[OptionName("Email")]
public class EmailOption
{
    [JsonPropertyName("Sender")] public EmailSenderOption Sender { get; set; } = null!;
    [JsonPropertyName("Smtp")] public EmailSmtpOption Smtp { get; set; } = null!;
}

public class EmailSenderOption
{
    [JsonPropertyName("Address")] public string Address { get; set; } = null!;
    [JsonPropertyName("Name")] public string Name { get; set; } = null!;
}

public class EmailSmtpOption
{
    [JsonPropertyName("Host")] public string Host { get; set; } = null!;
    [JsonPropertyName("Port")] public int Port { get; set; }
    [JsonPropertyName("Account")] public string Account { get; set; } = null!;
    [JsonPropertyName("Password")] public string Password { get; set; } = null!;
    [JsonPropertyName("TimeoutMs")] public int TimeoutMs { get; set; }
    [JsonPropertyName("UseSsl")] public bool UseSsl { get; set; }
    [JsonPropertyName("UseAuthentication")] public bool UseAuthentication { get; set; }
}
