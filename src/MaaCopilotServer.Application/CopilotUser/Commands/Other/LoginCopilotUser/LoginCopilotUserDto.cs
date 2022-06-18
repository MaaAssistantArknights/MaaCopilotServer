// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

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
    /// <param name="userInfo">The user information.</param>
    public LoginCopilotUserDto(string token, string validBefore, GetCopilotUserDto userInfo)
    {
        Token = token;
        ValidBefore = validBefore;
        UserInfo = userInfo;
    }

    /// <summary>
    /// The token expiration time.
    /// </summary>
    [JsonPropertyName("token")] public string Token { get; set; }

    /// <summary>
    /// The username.
    /// </summary>
    [JsonPropertyName("valid_before")] public string ValidBefore { get; set; }

    /// <summary>
    /// The user information.
    /// </summary>
    [JsonPropertyName("user_info")] public GetCopilotUserDto UserInfo { get; set; }
}