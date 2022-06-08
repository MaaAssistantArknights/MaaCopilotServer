// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

public class GetCopilotUserDto
{
    public GetCopilotUserDto(Guid id, string userName, UserRole userRole, int uploadCount)
    {
        Id = id;
        UserName = userName;
        UserRole = userRole;
        UploadCount = uploadCount;
    }

    [JsonPropertyName("id")] public Guid Id { get; set; }

    [JsonPropertyName("user_name")] public string UserName { get; set; }

    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole UserRole { get; set; }

    [JsonPropertyName("upload_count")] public int UploadCount { get; set; }
}
