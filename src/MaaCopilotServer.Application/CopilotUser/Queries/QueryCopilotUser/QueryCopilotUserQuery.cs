// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

/// <summary>
///     The DTO for the copilot user query.
/// </summary>
public record QueryCopilotUserQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The page number to query. Default is 1.
    /// </summary>
    [FromQuery(Name = "page")]
    public int? Page { get; set; } = null;

    /// <summary>
    ///     The max amount of items in a page. Default is 10.
    /// </summary>
    [FromQuery(Name = "limit")]
    public int? Limit { get; set; } = null;

    /// <summary>
    ///     The username to query.
    /// </summary>
    [FromQuery(Name = "user_name")]
    public string? UserName { get; set; } = null;
}

public class QueryCopilotUserQueryHandler : IRequestHandler<QueryCopilotUserQuery, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;

    public QueryCopilotUserQueryHandler(
        IMaaCopilotDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MaaApiResponse> Handle(QueryCopilotUserQuery request,
        CancellationToken cancellationToken)
    {
        // Pagination params
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;

        // Build the queryable
        var queryable = _dbContext.CopilotUsers.AsQueryable();

        if (string.IsNullOrEmpty(request.UserName) is false)
        {
            // If the user name is set, filter by it
            queryable = queryable.Where(x => x.UserName.Contains(request.UserName));
        }

        // Count total items
        var totalCount = await queryable.CountAsync(cancellationToken);

        // Pagination param
        var skip = (page - 1) * limit;

        // Order by the user creation time
        // Get the right page of items
        queryable = queryable
            .OrderByDescending(x => x.CreateAt)
            .Skip(skip).Take(limit);

        // Get all items
        var result = queryable.ToList();

        // Check if there are more pages or not
        var hasNext = request.Limit * request.Page < totalCount;

        // Build DTO
        var dtos = result
            .Select(x => new QueryCopilotUserDto(x.EntityId, x.UserName, x.UserRole))
            .ToList();

        // Build pagination DTO
        var paginationResult = new PaginationResult<QueryCopilotUserDto>
        {
            HasNext = hasNext,
            Page = page,
            Total = totalCount,
            Data = dtos,
        };
        return MaaApiResponseHelper.Ok(paginationResult);
    }
}
