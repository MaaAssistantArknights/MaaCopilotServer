// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

/// <summary>
///     The record of querying multiple users.
/// </summary>
public record QueryCopilotUserQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The page number to query.
    /// </summary>
    [FromQuery(Name = "page")]
    public int? Page { get; set; } = null;

    /// <summary>
    ///     The limitation of number of items in a page.
    /// </summary>
    [FromQuery(Name = "limit")]
    public int? Limit { get; set; } = null;

    /// <summary>
    ///     The username to query.
    /// </summary>
    [FromQuery(Name = "user_name")]
    public string? UserName { get; set; } = null;
}

/// <summary>
///     The handler of querying multiple users.
/// </summary>
public class QueryCopilotUserQueryHandler : IRequestHandler<QueryCopilotUserQuery,
    MaaApiResponse>
{
    /// <summary>
    ///     The service for current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext;

    /// <summary>
    ///     The constructor of <see cref="QueryCopilotUserQueryHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="currentUserService">The service for current user.</param>
    public QueryCopilotUserQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Handles a request of querying multiple users.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with an array of multiple users, excluding <c>upload_count</c>.</returns>
    public async Task<MaaApiResponse> Handle(QueryCopilotUserQuery request,
        CancellationToken cancellationToken)
    {
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;
        var queryable = _dbContext.CopilotUsers.AsQueryable();
        if (string.IsNullOrEmpty(request.UserName) is false)
        {
            queryable = queryable.Where(x => x.UserName.Contains(request.UserName));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var skip = (page - 1) * limit;
        queryable = queryable
            .OrderByDescending(x => x.CreateAt)
            .Skip(skip).Take(limit);

        var result = queryable.ToList();
        var hasNext = request.Limit * request.Page >= totalCount;

        var dtos = result
            .Select(x => new QueryCopilotUserDto(x.EntityId, x.UserName, x.UserRole))
            .ToList();
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
