// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

/// <summary>
/// The DTO of querying multiple users.
/// </summary>
public class QueryCopilotUserDto
{
    /// <summary>
    /// The constructor of <see cref="QueryCopilotUserDto"/>.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="userName">The username.</param>
    /// <param name="userRole">The role of the user.</param>
    public QueryCopilotUserDto(Guid id, string userName, UserRole userRole)
    {
        Id = id;
        UserName = userName;
        UserRole = userRole;
    }

    /// <summary>
    /// The user ID.
    /// </summary>
    [JsonPropertyName("id")] public Guid Id { get; set; }

    /// <summary>
    /// The username.
    /// </summary>
    [JsonPropertyName("user_name")] public string UserName { get; set; }

    /// <summary>
    /// The role of the user.
    /// </summary>
    [JsonPropertyName("role")][JsonConverter(typeof(JsonStringEnumConverter))] public UserRole UserRole { get; set; }
}
