// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

public record QueryCopilotUserQuery : IRequest<MaaActionResult<PaginationResult<QueryCopilotUserDto>>>
{
    [FromQuery(Name = "page")] public int? Page { get; set; } = null;
    [FromQuery(Name = "limit")] public int? Limit { get; set; } = null;
    [FromQuery(Name = "user_name")] public string? UserName { get; set; } = null;
}

public class QueryCopilotUserQueryHandler : IRequestHandler<QueryCopilotUserQuery,
    MaaActionResult<PaginationResult<QueryCopilotUserDto>>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public QueryCopilotUserQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<PaginationResult<QueryCopilotUserDto>>> Handle(QueryCopilotUserQuery request,
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
        var paginationResult = new PaginationResult<QueryCopilotUserDto>(hasNext, page, totalCount, dtos);
        return MaaApiResponse.Ok(paginationResult, _currentUserService.GetTrackingId());
    }
}
