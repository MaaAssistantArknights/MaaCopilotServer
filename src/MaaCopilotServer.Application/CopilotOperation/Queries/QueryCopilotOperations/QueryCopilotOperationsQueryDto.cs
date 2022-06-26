// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
///     The DTO of querying multiple operations.
/// </summary>
public class QueryCopilotOperationsQueryDto
{
    /// <summary>
    ///     The constructor of <see cref="QueryCopilotOperationsQueryDto" />.
    /// </summary>
    /// <param name="id">The operation ID.</param>
    /// <param name="stageName">The stage name.</param>
    /// <param name="minimumRequired">The minimum required version of MAA.</param>
    /// <param name="uploadTime">The time when the operation was uploaded.</param>
    /// <param name="uploader">The name of the uploader.</param>
    /// <param name="title">The title of the operation.</param>
    /// <param name="detail">The detail of the operation.</param>
    /// <param name="viewCounts">The view counts of the operation.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    public QueryCopilotOperationsQueryDto(string id, string stageName, string minimumRequired, string uploadTime,
        string uploader, string title, string detail, int viewCounts, IEnumerable<string> operators,
        IEnumerable<MaaCopilotOperationGroupStore> groups)
    {
        Id = id;
        StageName = stageName;
        MinimumRequired = minimumRequired;
        UploadTime = uploadTime;
        Uploader = uploader;
        Title = title;
        Detail = detail;
        ViewCounts = viewCounts;
        Operators = operators;
        Groups = groups;
    }

#pragma warning disable CS8618
    public QueryCopilotOperationsQueryDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The operation ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The stage name.
    /// </summary>
    [JsonPropertyName("stage_name")]
    public string StageName { get; set; }

    /// <summary>
    ///     The minimum required version of MAA.
    /// </summary>
    [JsonPropertyName("minimum_required")]
    public string MinimumRequired { get; set; }

    /// <summary>
    ///     The time when the operation was uploaded.
    /// </summary>
    [JsonPropertyName("upload_time")]
    public string UploadTime { get; set; }

    /// <summary>
    ///     The name of the uploader.
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    ///     The title of the operation.
    /// </summary>
    [JsonPropertyName("detail")]
    public string Detail { get; set; }

    /// <summary>
    ///     The detail of the operation.
    /// </summary>
    [JsonPropertyName("uploader")]
    public string Uploader { get; set; }

    /// <summary>
    ///     Operators used.
    /// </summary>
    [JsonPropertyName("operators")]
    public IEnumerable<string> Operators { get; set; }

    /// <summary>
    ///     Groups used.
    /// </summary>
    [JsonPropertyName("groups")]
    public IEnumerable<MaaCopilotOperationGroupStore> Groups { get; set; }

    /// <summary>
    ///     The number of times of views.
    /// </summary>
    [JsonPropertyName("views")]
    public int ViewCounts { get; set; }
}
