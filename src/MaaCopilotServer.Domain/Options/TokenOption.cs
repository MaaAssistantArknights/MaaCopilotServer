// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

/// <summary>
///     The <c>Token</c> Option
/// </summary>
[OptionName("Token")]
public class TokenOption
{
    /// <summary>
    ///     Account Activation Token configuration
    /// </summary>
    [JsonPropertyName("AccountActivationToken")]
    public TokenConfiguration AccountActivationToken { get; set; } = null!;

    /// <summary>
    ///     Password Reset Token configuration
    /// </summary>
    [JsonPropertyName("PasswordResetToken")]
    public TokenConfiguration PasswordResetToken { get; set; } = null!;

    /// <summary>
    ///     Change Email Token configuration
    /// </summary>
    [JsonPropertyName("ChangeEmailToken")]
    public TokenConfiguration ChangeEmailToken { get; set; } = null!;
}

public class TokenConfiguration
{
    /// <summary>
    ///     Token expiration time in minutes
    /// </summary>
    [JsonPropertyName("ExpireTime")]
    public int ExpireTime { get; set; }

    /// <summary>
    ///     Whether pass HasCallback parameter with true value of the email render
    /// </summary>
    [JsonPropertyName("HasCallback")]
    public bool HasCallback { get; set; }
}
