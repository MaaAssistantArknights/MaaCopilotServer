// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

public record QueryCopilotOperationsQuery : IRequest<MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>>
{
    [FromQuery(Name = "page")] public int Page { get; set; } = 1;
    [FromQuery(Name = "limit")] public int Limit { get; set; } = 10;
    [FromQuery(Name = "stage_name")] public string StageName { get; set; } = string.Empty;
    [FromQuery(Name = "content")] public string Content { get; set; } = string.Empty;
}

public class QueryCopilotOperationsQueryHandler : IRequestHandler<QueryCopilotOperationsQuery,
    MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;

    public QueryCopilotOperationsQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICopilotIdService copilotIdService,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _copilotIdService = copilotIdService;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>> Handle(QueryCopilotOperationsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _dbContext.CopilotOperations.AsQueryable();
        if (string.IsNullOrEmpty(request.StageName) is false)
        {
            queryable = queryable.Where(x => x.StageName.Contains(request.StageName));
        }
        if (string.IsNullOrEmpty(request.Content) is false)
        {
            queryable = queryable.Where(x => x.Content.Contains(request.Content));
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var skip = (request.Page - 1) * request.Limit;
        queryable = queryable.Skip(skip).Take(request.Limit);

        var result = queryable.ToList();
        var hasNext = request.Limit * request.Page >= totalCount;

        var dtos = result.Select(x => new QueryCopilotOperationsQueryDto(
                _copilotIdService.GetCopilotId(x.EntityId), x.StageName, x.MinimumRequired, x.CreateAt.ToStringZhHans(),
                x.Author.UserName))
            .ToList();
        var paginationResult = new PaginationResult<QueryCopilotOperationsQueryDto>(hasNext, request.Page, totalCount, dtos);
        return MaaApiResponse.Ok(paginationResult, _currentUserService.GetTrackingId());
    }
}
