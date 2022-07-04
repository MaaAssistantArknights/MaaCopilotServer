// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

/// <summary>
///     The response of the <see cref="LoginCopilotUserCommand"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class LoginCopilotUserDto
{
    /// <summary>
    ///     The constructor of <see cref="LoginCopilotUserDto" />.
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

#pragma warning disable CS8618
    public LoginCopilotUserDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The JWT token.
    /// </summary>
    [Required]
    [JsonPropertyName("token")]
    public string Token { get; set; }

    /// <summary>
    ///     The token expiration time.
    /// </summary>
    [Required]
    [JsonPropertyName("valid_before")]
    public string ValidBefore { get; set; }

    /// <summary>
    ///     The user information. Same as <see cref="GetCopilotUserDto"/>.
    /// </summary>
    [Required]
    [JsonPropertyName("user_info")]
    public GetCopilotUserDto UserInfo { get; set; }
}
