// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.RatingCopilotOperation;

/// <summary>
///     The response of the <see cref="RatingCopilotOperationCommand"/>.
/// </summary>
public record RatingCopilotOperationDto
{
#pragma warning disable CS8618
    // ReSharper disable once EmptyConstructor
    public RatingCopilotOperationDto() { }
#pragma warning restore CS8618
    
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
}
