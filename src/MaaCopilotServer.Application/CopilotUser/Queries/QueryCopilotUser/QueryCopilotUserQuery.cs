// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;

public record QueryCopilotUserQuery : IRequest<MaaActionResult<PaginationResult<QueryCopilotUserDto>>>
{
    [FromQuery(Name = "page")] public int Page { get; set; } = 1;
    [FromQuery(Name = "limit")] public int Limit { get; set; } = 10;
    [FromQuery(Name = "user_name")] public string UserName { get; set; } = string.Empty;
}

public class QueryCopilotUserQueryHandler : IRequestHandler<QueryCopilotUserQuery, MaaActionResult<PaginationResult<QueryCopilotUserDto>>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public QueryCopilotUserQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<PaginationResult<QueryCopilotUserDto>>> Handle(QueryCopilotUserQuery request, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.CopilotUsers.AsQueryable();
        if (string.IsNullOrEmpty(request.UserName) is false)
        {
            queryable = queryable.Where(x => x.UserName.Contains(request.UserName));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var skip = (request.Page - 1) * request.Limit;
        queryable = queryable.Skip(skip).Take(request.Limit);

        var result = queryable.ToList();
        var hasNext = request.Limit * request.Page >= totalCount;

        var dtos = result
            .Select(x => new QueryCopilotUserDto(x.EntityId, x.UserName, x.UserRole))
            .ToList();
        var paginationResult = new PaginationResult<QueryCopilotUserDto>(hasNext, request.Page, totalCount, dtos);
        return MaaApiResponse.Ok(paginationResult, _currentUserService.GetTrackingId());
    }
}
