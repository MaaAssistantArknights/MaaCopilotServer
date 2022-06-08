// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

public class GetCopilotOperationQueryDto
{
    public GetCopilotOperationQueryDto(string id, string stageName, string minimumRequired, string uploadTime,
        string content, string uploader, string title, string detail, int downloads)
    {
        Id = id;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        UploadTime = uploadTime;
        Content = content;
        Uploader = uploader;
        Title = title;
        Detail = detail;
        Downloads = downloads;
    }

    [JsonPropertyName("id")] public string Id { get; }

    [JsonPropertyName("stage_name")] public string StageName { get; }

    [JsonPropertyName("minimum_required")] public string MinimumRequired { get; }

    [JsonPropertyName("upload_time")] public string UploadTime { get; }

    [JsonPropertyName("content")] public string Content { get; }

    [JsonPropertyName("title")] public string Title { get; }

    [JsonPropertyName("detail")] public string Detail { get; }

    [JsonPropertyName("uploader")] public string Uploader { get; }
    [JsonPropertyName("downloads")] public int Downloads { get; }
}
