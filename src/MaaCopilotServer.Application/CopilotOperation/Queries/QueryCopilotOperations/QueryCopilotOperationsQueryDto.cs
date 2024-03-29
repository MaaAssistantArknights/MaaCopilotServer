// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Arknights.GetLevelList;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
///     The response to the <see cref="QueryCopilotOperationsQuery"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public class QueryCopilotOperationsQueryDto
{
#pragma warning disable CS8618
    // ReSharper disable once EmptyConstructor
    public QueryCopilotOperationsQueryDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The operation id.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; }

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
    [JsonPropertyName("hot_score")]
    public long HotScore { get; set; }

    /// <summary>
    ///     The level this operation is made for.
    /// </summary>
    [Required]
    [JsonPropertyName("level")]
    public GetLevelListDto Level { get; set; }

    /// <summary>
    ///     The level this operation is made for is available in your region.
    /// </summary>
    [Required]
    [JsonPropertyName("available")]
    public bool Available => string.IsNullOrEmpty(Level.Name) is false;

    /// <summary>
    ///     Current rating level.
    /// </summary>
    [Required]
    [JsonPropertyName("rating_level")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RatingLevel RatingLevel { get; set; }
    
    /// <summary>
    ///     Current rating ratio.
    /// </summary>
    [Required]
    [JsonPropertyName("rating_ratio")]
    public double RatingRatio { get; set; }
    
    /// <summary>
    ///     Is total rating count enough or not.
    /// </summary>
    [Required]
    [JsonPropertyName("is_not_enough_rating")]
    public bool IsNotEnoughRating { get; set; }

    /// <summary>
    ///     The rating type for this operation by current user. It will be null for anonymous user.
    /// </summary>
    [JsonPropertyName("rating_type")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperationRatingType? RatingType { get; set; }

    /// <summary>
    /// The level difficulty.
    /// </summary>
    [JsonPropertyName("difficulty")]
    public DifficultyType Difficulty { get; set; }
}
