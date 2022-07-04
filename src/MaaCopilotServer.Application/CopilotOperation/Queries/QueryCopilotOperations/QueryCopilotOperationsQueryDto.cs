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
    ///     The hot score.
    /// </summary>
    [Required]
    [JsonPropertyName("HotScore")]
    public long HotScore { get; set; }

    /// <summary>
    ///     Current rating level, i18n string.
    /// </summary>
    [Required]
    [JsonPropertyName("rating_level")]
    public string RatingLevel { get; set; }

    /// <summary>
    ///     The rating type for this operation by current user. It will be null for anonymous user.
    /// </summary>
    [JsonPropertyName("rating_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperationRatingType? RatingType { get; set; }
}
