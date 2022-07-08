// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Attributes;

namespace MaaCopilotServer.Domain.Options;

[OptionName("CopilotOperation")]
public class CopilotOperationOption
{
    // The hot score equation is:
    // 
    // HotScore =
    //     LIKES * {LikeMultiplier} +
    //     DISLIKE * {DislikeMultiplier} +
    //     VIEWS * {ViewMultiplier} +
    //     + {InitialHotScore}
    //

    /// <summary>
    ///     The multiplier for likes.
    /// </summary>
    [JsonPropertyName("LikeMultiplier")]
    public double LikeMultiplier { get; set; } = 1;

    /// <summary>
    ///     The multiplier for dislikes.
    /// </summary>
    [JsonPropertyName("DislikeMultiplier")]
    public double DislikeMultiplier { get; set; } = 1;

    /// <summary>
    ///     The multiplier for views.
    /// </summary>
    [JsonPropertyName("ViewMultiplier")]
    public double ViewMultiplier { get; set; } = 1;

    /// <summary>
    ///     The initial hot score.
    /// </summary>
    [JsonPropertyName("InitialHotScore")]
    public double InitialHotScore { get; set; } = 1;
}
