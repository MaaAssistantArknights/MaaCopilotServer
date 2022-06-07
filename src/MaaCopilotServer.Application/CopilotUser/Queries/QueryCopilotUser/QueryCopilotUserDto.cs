// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

public class QueryCopilotUserDto
{
    public QueryCopilotUserDto(Guid id, string userName, UserRole userRole)
    {
        Id = id;
        UserName = userName;
        UserRole = userRole;
    }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
    [JsonPropertyName("role")]
    public UserRole UserRole { get; set; }
}
