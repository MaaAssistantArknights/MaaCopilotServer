// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Models;

/// <summary>
///     The model of pagination.
/// </summary>
/// <typeparam name="T">The type of the result.</typeparam>
public record PaginationResult<T>
{
    /// <summary>
    ///     Indicates whether there are more pages.
    /// </summary>
    [JsonPropertyName("has_next")]
    public bool HasNext { get; set; }

    /// <summary>
    ///     The page number.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    ///     The number of pages in total.
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    ///     The response body.
    /// </summary>
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();
}
