// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Operation;
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
    ///     Desc or Asc. Default is Asc. Set this value to "true" to sort in descending order.
    /// </summary>
    [FromQuery(Name = "desc")]
    public string? Desc { get; set; } = null;

    /// <summary>
    ///     Orders result by a field. Only supports ordering by "views", "hot" and "id" (default).
    /// </summary>
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; } = null;

    /// <summary>
    ///     The server language. Could be (ignore case) Chinese (Default), English, Japanese, Korean.
    /// </summary>
    [FromQuery(Name = "server")]
    public string Server { get; set; } = string.Empty;
}

public class QueryCopilotOperationsQueryHandler : IRequestHandler<QueryCopilotOperationsQuery,
    MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICopilotOperationService _copilotOperationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public QueryCopilotOperationsQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICopilotOperationService copilotOperationService,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _copilotOperationService = copilotOperationService;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(
        QueryCopilotOperationsQuery request, CancellationToken cancellationToken)
    {
        // Get current infos
        var user = await _currentUserService.GetUser();
        var isLoggedIn = user is not null;

        // Set pagination params
        // limit and page are optional in request, so we need to check if they are set
        // if not, set the default values
        var limit = request.Limit ?? 10;
        var page = request.Page ?? 1;

        // Get uploader id
        Guid? uploaderId;
        if (request.UploaderId == "me")
        {
            // if uploader id is set to me, try to get current user id
            // if user is not logged in, return a bad request error
            var id = _currentUserService.GetUserIdentity();
            if (id is null)
            {
                return MaaApiResponseHelper.BadRequest(_apiErrorMessage.MeNotFound);
            }

            uploaderId = id.Value;
        }
        else if (string.IsNullOrEmpty(request.UploaderId))
        {
            // if uploader id is not set, set the value to null
            uploaderId = null;
        }
        else
        {
            // if the uploader if is set, set the value to the given id
            uploaderId = Guid.Parse(request.UploaderId);
        }

        // Build queryable
        var queryable = _dbContext.CopilotOperations
            .Include(x => x.ArkLevel)
            .Include(x => x.Author)
            .AsQueryable();
        if (string.IsNullOrEmpty(request.Content) is false)
        {
            // if content is set, filter by it
            queryable = queryable.Where(x => x.Content.Contains(request.Content));
        }
        if (string.IsNullOrEmpty(request.Uploader) is false)
        {
            // if uploader is set, filter by it
            queryable = queryable.Where(x => x.Author.UserName.Contains(request.Uploader));
        }

        if (uploaderId is not null)
        {
            // if uploader id is set, filter by it
            queryable = queryable.Where(x => x.Author.EntityId == uploaderId);
        }

        // Count total amount of items
        var totalCount = await queryable.CountAsync(cancellationToken);

        // Pagination value, skip some items to get the page we want
        var skip = (page - 1) * limit;

        // Order by
        queryable = request.OrderBy?.ToLower() switch
        {
            // if views is set, order by views
            "views" => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.ViewCounts)
                : queryable.OrderByDescending(x => x.ViewCounts),
            // if rating is set, order by rating
            "hot" => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.HotScore)
                : queryable.OrderByDescending(x => x.HotScore),
            // if no order is set, order by id
            _ => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.Id)
                : queryable.OrderByDescending(x => x.Id)
        };

        // Build full query
        queryable = queryable.Skip(skip).Take(limit);

        // Get all items
        var result = queryable.ToList();
        // Check if there are any more pages
        // current selected items = limit * page
        // if it is less then total count, there are more pages
        var hasNext = limit * page < totalCount;

        // TODO: Find more elegant way to do this.
        // If user is logged in, get the rating for each operation
        // If not, set the rating to an empty list
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
                    Id = _copilotOperationService.EncodeId(x.Id),
                    Detail = x.Details,
                    MinimumRequired = x.MinimumRequired,
                    Title = x.Title,
                    Uploader = x.Author.UserName,
                    HotScore = x.HotScore,
                    Groups = x.Groups.ToArray().DeserializeGroup(),
                    Operators = x.Operators,
                    UploadTime = x.UpdateAt.ToIsoString(),
                    ViewCounts = x.ViewCounts,
                    Level = x.ArkLevel.MapToDto(request.Server),
                    RatingLevel = _copilotOperationService.GetRatingLevelString(x.RatingLevel),
                    // If the user is logged in, get the rating for the operation, default value is None
                    // If not, set to null
                    RatingType = isLoggedIn
                        ? rating.FirstOrDefault(y => y.OperationId == x.EntityId)?
                              .RatingType ?? OperationRatingType.None
                        : null
                })
            .ToList();
        // Build pagination response
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
