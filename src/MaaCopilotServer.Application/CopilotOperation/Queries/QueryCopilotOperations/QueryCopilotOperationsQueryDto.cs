// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

public class QueryCopilotOperationsQueryDto
{
    public QueryCopilotOperationsQueryDto(string id, string stageName, string minimumRequired, string uploadTime, string uploader, string title, string detail)
    {
        Id = id;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        UploadTime = uploadTime;
        Uploader = uploader;
        Title = title;
        Detail = detail;
    }

    [JsonPropertyName("id")]
    public string Id { get; }
    [JsonPropertyName("stage_name")]
    public string StageName { get; }
    [JsonPropertyName("minimum_required")]
    public string MinimumRequired { get; }
    [JsonPropertyName("upload_time")]
    public string UploadTime { get; }
    [JsonPropertyName("title")]
    public string Title { get; }
    [JsonPropertyName("detail")]
    public string Detail { get; }
    [JsonPropertyName("uploader")]
    public string Uploader { get; }
}
