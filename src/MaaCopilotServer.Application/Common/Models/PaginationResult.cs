// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;

namespace MaaCopilotServer.Application.Common.Models;

public class PaginationResult<T>
{
    public PaginationResult(bool hasNext, int page, int total, List<T> data)
    {
        HasNext = hasNext;
        Page = page;
        Total = total;
        Data = data;
    }

    [JsonPropertyName("has_next")]
    public bool HasNext { get; set; }
    [JsonPropertyName("page")]
    public int Page { get; set; }
    [JsonPropertyName("total")]
    public int Total { get; set; }
    [JsonPropertyName("data")]
    public List<T> Data { get; set; }
}
