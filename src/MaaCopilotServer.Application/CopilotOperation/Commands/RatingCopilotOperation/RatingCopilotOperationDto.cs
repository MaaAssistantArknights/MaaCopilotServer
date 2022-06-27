// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.RatingCopilotOperation;

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

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("rating")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OperationRatingType RatingType { get; set; }

    [JsonPropertyName("rating_ratio")]
    public float CurrentRatio { get; set; }
}
