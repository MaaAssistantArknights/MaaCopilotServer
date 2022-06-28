// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
///     The response to the <see cref="QueryCopilotOperationsQuery"/>.
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
    /// <param name="ratingRatio">The like to all rating ratio.</param>
    /// <param name="operators">The operators in the operation.</param>
    /// <param name="groups">The groups in the operation.</param>
    /// <param name="ratingType">The rating type by current user.</param>
    public QueryCopilotOperationsQueryDto(string id, string stageName, string minimumRequired, string uploadTime,
        string uploader, string title, string detail, int viewCounts, float ratingRatio,
        IEnumerable<string> operators, IEnumerable<MaaCopilotOperationGroupStore> groups,
        OperationRatingType? ratingType = null)
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
        RatingRatio = ratingRatio;
        RatingType = ratingType;
    }

#pragma warning disable CS8618
    public QueryCopilotOperationsQueryDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The operation id.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The stage name.
    /// </summary>
    [Required]
    [JsonPropertyName("stage_name")]
    public string StageName { get; set; }

    /// <summary>
    ///     The minimum required version of MAA.
    /// </summary>
    [Required]
    [JsonPropertyName("minimum_required")]
    public string MinimumRequired { get; set; }

    /// <summary>
    ///     The time when the operation was uploaded.
    /// </summary>
    [Required]
    [JsonPropertyName("upload_time")]
    public string UploadTime { get; set; }

    /// <summary>
    ///     The name of the uploader.
    /// </summary>
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    /// <summary>
    ///     The title of the operation.
    /// </summary>
    [Required]
    [JsonPropertyName("detail")]
    public string Detail { get; set; }

    /// <summary>
    ///     The detail of the operation.
    /// </summary>
    [Required]
    [JsonPropertyName("uploader")]
    public string Uploader { get; set; }

    /// <summary>
    ///     Operators used.
    /// </summary>
    [Required]
    [JsonPropertyName("operators")]
    public IEnumerable<string> Operators { get; set; }

    /// <summary>
    ///     Groups used.
    /// </summary>
    [Required]
    [JsonPropertyName("groups")]
    public IEnumerable<MaaCopilotOperationGroupStore> Groups { get; set; }

    /// <summary>
    ///     The number of times of views.
    /// </summary>
    [Required]
    [JsonPropertyName("views")]
    public int ViewCounts { get; set; }

    /// <summary>
    ///     The rating ratio.
    /// </summary>
    [Required]
    [JsonPropertyName("rating_ratio")]
    public float RatingRatio { get; set; }

    /// <summary>
    ///     The rating type for this operation by current user. It will be null for anonymous user.
    /// </summary>
    [JsonPropertyName("rating_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperationRatingType? RatingType { get; set; }
}
