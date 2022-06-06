// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

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

    public bool HasNext { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
    public List<T> Data { get; set; }
}
