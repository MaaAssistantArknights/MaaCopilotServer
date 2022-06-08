// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

public class LoginCopilotUserDto
{
    public LoginCopilotUserDto(string token, string validBefore, string userName)
    {
        Token = token;
        ValidBefore = validBefore;
        UserName = userName;
    }

    [JsonPropertyName("user_name")] public string UserName { get; set; }

    [JsonPropertyName("token")] public string Token { get; set; }

    [JsonPropertyName("valid_before")] public string ValidBefore { get; set; }
}
