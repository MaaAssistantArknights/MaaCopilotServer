// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

public record QueryCopilotOperationsQuery : IRequest<MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>>
{
    [FromQuery(Name = "page")] public int? Page { get; set; } = null;
    [FromQuery(Name = "limit")] public int? Limit { get; set; } = null;
    [FromQuery(Name = "stage_name")] public string? StageName { get; set; } = null;
    [FromQuery(Name = "content")] public string? Content { get; set; } = null;
    [FromQuery(Name = "uploader")] public string? Uploader { get; set; } = null;
    [FromQuery(Name = "uploader_id")] public string? UploaderId { get; set; } = null;
}

public class QueryCopilotOperationsQueryHandler : IRequestHandler<QueryCopilotOperationsQuery,
    MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>>
{
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public QueryCopilotOperationsQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICopilotIdService copilotIdService,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _copilotIdService = copilotIdService;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>> Handle(
        QueryCopilotOperationsQuery request, CancellationToken cancellationToken)
    {
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;
        Guid? uploaderId;
        if (request.UploaderId == "me")
        {
            var id = _currentUserService.GetUserIdentity();
            if (id is null)
            {
                return MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(), "User is not authenticated.");
            }

            uploaderId = id.Value;
        }
        else if (string.IsNullOrEmpty(request.UploaderId))
        {
            uploaderId = null;
        }
        else
        {
            uploaderId = Guid.Parse(request.UploaderId);
        }

        var queryable = _dbContext.CopilotOperations.Include(x => x.Author).AsQueryable();
        if (string.IsNullOrEmpty(request.StageName) is false)
        {
            queryable = queryable.Where(x => x.StageName.Contains(request.StageName));
        }

        if (string.IsNullOrEmpty(request.Content) is false)
        {
            queryable = queryable.Where(x => x.Content.Contains(request.Content));
        }

        if (string.IsNullOrEmpty(request.Uploader) is false)
        {
            queryable = queryable.Where(x => x.Author.UserName.Contains(request.Uploader));
        }

        if (uploaderId is not null)
        {
            queryable = queryable.Where(x => x.Author.EntityId == uploaderId);
        }

        var totalCount = await queryable.CountAsync(cancellationToken);

        var skip = (page - 1) * limit;
        queryable = queryable.Skip(skip).Take(limit);

        var result = queryable.ToList();
        var hasNext = limit * page >= totalCount;

        var dtos = result.Select(x => new QueryCopilotOperationsQueryDto(
                _copilotIdService.EncodeId(x.Id), x.StageName, x.MinimumRequired, x.CreateAt.ToStringZhHans(),
                x.Author.UserName, x.Title, x.Details))
            .ToList();
        var paginationResult = new PaginationResult<QueryCopilotOperationsQueryDto>(hasNext, page, totalCount, dtos);
        return MaaApiResponse.Ok(paginationResult, _currentUserService.GetTrackingId());
    }
}
