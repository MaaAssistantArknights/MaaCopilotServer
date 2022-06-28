// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.RatingCopilotOperation;

/// <summary>
///     The response to a <see cref="RatingCopilotOperationCommand" />.
/// </summary>
public record RatingCopilotOperationDto
{
    public RatingCopilotOperationDto(string id, OperationRatingType ratingType, float currentRatio)
    {
        Id = id;
        RatingType = ratingType;
        CurrentRatio = currentRatio;
    }

#pragma warning disable CS8618
    public RatingCopilotOperationDto() { }
#pragma warning restore CS8618

    /// <summary>
    ///     The id of the operation.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    ///     The type of the rating.
    /// </summary>
    /// <remarks>Value range: Like, Dislike, None</remarks>
    [Required]
    [JsonPropertyName("rating")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperationRatingType RatingType { get; set; }

    /// <summary>
    ///     The current ratio of the operation. If the operation is not rated, the value is -1. Only 4 digits at max will be kept.
    /// </summary>
    /// <remarks>Ratio = Like / (Like + Dislike)</remarks>
    [Required]
    [JsonPropertyName("rating_ratio")]
    public float CurrentRatio { get; set; }
}
