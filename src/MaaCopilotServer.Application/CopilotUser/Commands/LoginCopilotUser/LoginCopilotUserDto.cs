// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

/// <summary>
/// The DTO of user login.
/// </summary>
public class LoginCopilotUserDto
{
    /// <summary>
    /// The constructor of <see cref="LoginCopilotUserDto"/>.
    /// </summary>
    /// <param name="token">The user token.</param>
    /// <param name="validBefore">The token expiration time.</param>
    /// <param name="userName">The username.</param>
    public LoginCopilotUserDto(string token, string validBefore, string userName)
    {
        Token = token;
        ValidBefore = validBefore;
        UserName = userName;
    }

    /// <summary>
    /// The user token.
    /// </summary>
    [JsonPropertyName("user_name")] public string UserName { get; set; }

    /// <summary>
    /// The token expiration time.
    /// </summary>
    [JsonPropertyName("token")] public string Token { get; set; }

    /// <summary>
    /// The username.
    /// </summary>
    [JsonPropertyName("valid_before")] public string ValidBefore { get; set; }
}
