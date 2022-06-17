// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

/// <summary>
/// The DTO of getting user.
/// </summary>
public class GetCopilotUserDto
{
    /// <summary>
    /// The constructor of <see cref="GetCopilotUserDto"/>.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="userName">The username.</param>
    /// <param name="userRole">The role of the user.</param>
    /// <param name="uploadCount">The number of uploads.</param>
    public GetCopilotUserDto(Guid id, string userName, UserRole userRole, int uploadCount)
    {
        Id = id;
        UserName = userName;
        UserRole = userRole;
        UploadCount = uploadCount;
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
    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole UserRole { get; set; }

    /// <summary>
    /// The number of uploads.
    /// </summary>
    [JsonPropertyName("upload_count")] public int UploadCount { get; set; }
}
