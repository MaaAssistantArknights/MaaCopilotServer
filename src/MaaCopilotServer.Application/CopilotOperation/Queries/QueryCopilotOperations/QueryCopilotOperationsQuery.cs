// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
///     The DTO for the query copilot operations.
/// </summary>
public record QueryCopilotOperationsQuery : IRequest<MaaApiResponse>
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
    ///     The stage name to query.
    /// </summary>
    [FromQuery(Name = "stage_name")]
    public string? StageName { get; set; } = null;

    /// <summary>
    ///     The content to query.
    /// </summary>
    [FromQuery(Name = "content")]
    public string? Content { get; set; } = null;

    /// <summary>
    ///     The name of the uploader.
    /// </summary>
    [FromQuery(Name = "uploader")]
    public string? Uploader { get; set; } = null;

    /// <summary>
    ///     The ID of the uploader
    /// </summary>
    [FromQuery(Name = "uploader_id")]
    public string? UploaderId { get; set; } = null;

    /// <summary>
    ///     The description to query.
    /// </summary>
    [FromQuery(Name = "desc")]
    public string? Desc { get; set; } = null;

    /// <summary>
    ///     Orders result by a field. Only supports ordering by "views", "rating" and "id" (default).
    /// </summary>
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; } = null;
}

public class QueryCopilotOperationsQueryHandler : IRequestHandler<QueryCopilotOperationsQuery,
    MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public QueryCopilotOperationsQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICopilotIdService copilotIdService,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _copilotIdService = copilotIdService;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(
        QueryCopilotOperationsQuery request, CancellationToken cancellationToken)
    {
        var user = await _currentUserService.GetUser();
        var isLoggedIn = user is not null;
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;
        Guid? uploaderId;
        if (request.UploaderId == "me")
        {
            var id = _currentUserService.GetUserIdentity();
            if (id is null)
            {
                return MaaApiResponseHelper.BadRequest(_apiErrorMessage.MeNotFound);
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

        queryable = request.OrderBy?.ToLower() switch
        {
            "views" => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.ViewCounts)
                : queryable.OrderByDescending(x => x.ViewCounts),
            "rating" => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.RatingRatio)
                : queryable.OrderByDescending(x => x.RatingRatio),
            _ => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.Id)
                : queryable.OrderByDescending(x => x.Id)
        };

        queryable = queryable.Skip(skip).Take(limit);

        var result = queryable.ToList();
        var hasNext = limit * page < totalCount;

        // TODO: Find more elegant way to do this.
        var rating = isLoggedIn
            ? (await _dbContext.CopilotOperationRatings
                .Where(x => x.UserId == user!.EntityId)
                .ToListAsync(cancellationToken))
                .Where(x => result.Any(y => y.EntityId == x.OperationId))
                .ToList()
            : new List<CopilotOperationRating>();

        var dtos = result.Select(x =>
                new QueryCopilotOperationsQueryDto
                {
                    Id = _copilotIdService.EncodeId(x.Id),
                    Detail = x.Details,
                    MinimumRequired = x.MinimumRequired,
                    StageName = x.StageName,
                    Title = x.Title,
                    Uploader = x.Author.UserName,
                    RatingRatio = x.RatingRatio,
                    Groups = x.Groups.ToArray().DeserializeGroup(),
                    Operators = x.Operators,
                    UploadTime = x.UpdateAt.ToIsoString(),
                    ViewCounts = x.ViewCounts,
                    RatingType = isLoggedIn
                        ? rating.FirstOrDefault(y => y.OperationId == x.EntityId)?
                              .RatingType ?? OperationRatingType.None
                        : null
                })
            .ToList();
        var paginationResult = new PaginationResult<QueryCopilotOperationsQueryDto>
        {
            HasNext = hasNext,
            Page = page,
            Total = totalCount,
            Data = dtos,
        };
        return MaaApiResponseHelper.Ok(paginationResult);
    }
}
