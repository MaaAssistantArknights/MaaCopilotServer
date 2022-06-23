// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;

/// <summary>
///     The record of querying multiple operations.
/// </summary>
public record QueryCopilotOperationsQuery : IRequest<MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>>
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
    ///     The stage name.
    /// </summary>
    [FromQuery(Name = "stage_name")]
    public string? StageName { get; set; } = null;

    /// <summary>
    ///     The content.
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
    ///     The description.
    /// </summary>
    [FromQuery(Name = "desc")]
    public string? Desc { get; set; } = null;

    /// <summary>
    ///     Orders result by a field. Only supports ordering by <c>downloads</c> and <c>id</c> (default).
    /// </summary>
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; } = null;
}

/// <summary>
///     The handler of querying multiple operations.
/// </summary>
public class QueryCopilotOperationsQueryHandler : IRequestHandler<QueryCopilotOperationsQuery,
    MaaActionResult<PaginationResult<QueryCopilotOperationsQueryDto>>>
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage;

    /// <summary>
    ///     The service for processing copilot ID.
    /// </summary>
    private readonly ICopilotIdService _copilotIdService;

    /// <summary>
    ///     The service for current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext;

    /// <summary>
    ///     The constructor of <see cref="QueryCopilotOperationsQueryHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="copilotIdService">The service for processing copilot ID.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
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

    /// <summary>
    ///     Handles a request of querying multiple operations.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with an array of multiple operations without contents.</returns>
    /// <exception cref="PipelineException">Thrown when the uploader ID does not exist.</exception>
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
                throw new PipelineException(MaaActionResultHelper.BadRequest(_currentUserService.GetTrackingId(),
                    _apiErrorMessage.MeNotFound));
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
            "downloads" => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.Downloads)
                : queryable.OrderByDescending(x => x.Downloads),
            _ => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.Id)
                : queryable.OrderByDescending(x => x.Id)
        };

        queryable = queryable.Skip(skip).Take(limit);

        var result = queryable.ToList();
        var hasNext = limit * page >= totalCount;

        var dtos = result.Select(x => new QueryCopilotOperationsQueryDto(
                _copilotIdService.EncodeId(x.Id), x.StageName, x.MinimumRequired,
                x.CreateAt.ToString("o", _apiErrorMessage.CultureInfo),
                x.Author.UserName, x.Title, x.Details, x.Downloads, x.Operators))
            .ToList();
        var paginationResult = new PaginationResult<QueryCopilotOperationsQueryDto>(hasNext, page, totalCount, dtos);
        return MaaActionResultHelper.Ok<PaginationResult<QueryCopilotOperationsQueryDto>>(paginationResult, _currentUserService.GetTrackingId());
    }
}
