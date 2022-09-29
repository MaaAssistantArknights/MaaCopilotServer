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
    public int? Page { get; set; }

    /// <summary>
    ///     The max amount of items in a page. Default is 10.
    /// </summary>
    [FromQuery(Name = "limit")]
    public int? Limit { get; set; }

    /// <summary>
    ///     The key word to search for. Set this field, the query will run
    /// on all Categories and LevelNames.
    /// </summary>
    [FromQuery(Name = "level_keyword")]
    public string? Keyword { get; set; }

    /// <summary>
    ///     The Title and Details to search for.
    /// </summary>
    [FromQuery(Name = "document")]
    public string? Document { get; set; } = null;

    /// <summary>
    ///     The operator query string. Use `,` to split multiple expressions. All expressions will be combined with AND.
    /// The expression defaults to `Include`, add `~` at the beginning to perform an `Exclude` operation. Eg: `A,~B,C`
    /// will be translate to `Must include A and C, and must exclude B`. The operator query will not be only applied to
    /// the `Operators` field of copilot operation. It will not be applied to the `Groups` field, because this field is
    /// too complex to perform a query on. Be aware that `Exclude` operation is performed before `Include` operation.
    /// </summary>
    [FromQuery(Name = "operator")]
    public string? Operator { get; set; } = null;

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
    public string? UploaderId { get; set; }

    /// <summary>
    ///     Desc or Asc. Default is Asc. Set this value to "true" to sort in descending order.
    /// </summary>
    [FromQuery(Name = "desc")]
    public string? Desc { get; set; }

    /// <summary>
    ///     Orders result by a field. Only supports ordering by "views", "hot" and "id" (default).
    /// </summary>
    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; }

    /// <summary>
    ///     The server language.
    /// <para>Options: </para>
    /// <para>Chinese (China Mainland) - zh_cn, cn</para>
    /// <para>Chinese (Taiwan, China) - zh_tw, tw</para>
    /// <para>English (Global) - en_us, en</para>
    /// <para>Japanese (Japan) - ja_jp, ja</para>
    /// <para>Korean (South Korea) - ko_kr, ko</para>
    /// </summary>
    [FromQuery(Name = "language")]
    public string Language { get; set; } = string.Empty;
}

public class QueryCopilotOperationsQueryHandler : IRequestHandler<QueryCopilotOperationsQuery,
    MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public QueryCopilotOperationsQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
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
            .Include(x => x.ArkLevel).ThenInclude(x => x.Keyword)
            .Include(x => x.ArkLevel).ThenInclude(x => x.CatOne)
            .Include(x => x.ArkLevel).ThenInclude(x => x.CatTwo)
            .Include(x => x.ArkLevel).ThenInclude(x => x.CatThree)
            .Include(x => x.ArkLevel).ThenInclude(x => x.Name)
            .Include(x => x.Author)
            .AsQueryable();
        if (string.IsNullOrEmpty(request.Content) is false)
        {
            // if content is set, filter by it
            queryable = queryable.Where(x => EF.Functions.ILike(x.Content, $"%{request.Content}%"));
        }
        if (string.IsNullOrEmpty(request.Uploader) is false)
        {
            // if uploader is set, filter by it
            queryable = queryable.Where(x => EF.Functions.ILike(x.Author.UserName, $"%{request.Uploader}%"));
        }

        if (string.IsNullOrEmpty(request.Keyword) is false)
        {
            // if keyword is set, filter by it
            queryable = request.Language.GetQueryKeywordFunc().Invoke(queryable, request.Keyword);
        }

        if (string.IsNullOrEmpty(request.Document) is false)
        {
            // if document is set, filter by it
            // match both Title and Details fields
            queryable = queryable.Where(x =>
                EF.Functions.ILike(x.Title, $"%{request.Document}%") ||
                EF.Functions.ILike(x.Details, $"%{request.Document}%"));
        }

        if (uploaderId is not null)
        {
            // if uploader id is set, filter by it
            queryable = queryable.Where(x => x.Author.EntityId == uploaderId);
        }

        // Pagination value, skip some items to get the page we want
        var skip = (page - 1) * limit;

        // Order by
        queryable = request.OrderBy?.ToLower() switch
        {
            // if views is set, order by views
            "views" => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.ViewCounts).ThenBy(x => x.Id)
                : queryable.OrderByDescending(x => x.ViewCounts).ThenByDescending(x => x.Id),
            // if rating is set, order by rating
            "hot" => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.HotScore).ThenBy(x => x.Id)
                : queryable.OrderByDescending(x => x.HotScore).ThenByDescending(x => x.Id),
            _ => string.IsNullOrEmpty(request.Desc)
                ? queryable.OrderBy(x => x.Id)
                : queryable.OrderByDescending(x => x.Id)
        };

        int totalCount;
        List<Domain.Entities.CopilotOperation> result;
        bool hasNext;

        if (string.IsNullOrEmpty(request.Operator))
        {
            // Count total amount of items
            totalCount = await queryable.CountAsync(cancellationToken);
        
            // Build full query
            queryable = queryable.Skip(skip).Take(limit);

            // Get all items
            result = queryable.ToList();
            
            // Check if there are any more pages
            // current selected items = limit * page
            // if it is less then total count, there are more pages
            hasNext = limit * page < totalCount;
        }
        else
        {
            // TODO: Need a better way to do this
            
            var conditions = request.Operator.Split(",");
            var exclude = conditions
                .Where(x => x.Length > 1)
                .Where(x => x.StartsWith("~"))
                .Select(x => x[1..]);
            var include = conditions
                .Where(x => x.Length > 0)
                .Where(x => x.StartsWith("~") is false);

            // The following aggregated complex query can not be translated to SQL.
            // Execute SQL query to convert the query from server-evaluation to
            // client-evaluation, so that the following query could be executed.
            // Be aware that these kind of client-evaluation queries may have
            // performance impact with an extremely large data set.
            
            var e = queryable.AsEnumerable();

            e = exclude.Aggregate(e,
                (current, condition) => 
                    current.Where(x =>
                        x.Operators.Any(y => y.Contains(condition) is false)));
            e = include.Aggregate(e,
                (current, condition) =>
                    current.Where(x =>
                        x.Operators.Any(y => y.Contains(condition))));
            var eResult = e.ToArray();

            totalCount = eResult.Length;
            result = eResult.Skip(skip).Take(limit).ToList();
            hasNext = limit * page < totalCount;
        }

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
                    Id = EntityIdHelper.EncodeId(x.Id),
                    Detail = x.Details,
                    MinimumRequired = x.MinimumRequired,
                    Title = x.Title,
                    Uploader = x.Author.UserName,
                    HotScore = x.HotScore,
                    Groups = x.Groups.ToArray().DeserializeGroup(),
                    Operators = x.Operators,
                    UploadTime = x.CreateAt.ToIsoString(),
                    ViewCounts = x.ViewCounts,
                    Level = request.Language.GetLevelMapperFunc().Invoke(x.ArkLevel),
                    RatingLevel = x.RatingLevel,
                    RatingRatio = x.RatingRatio,
                    IsNotEnoughRating = x.IsNotEnoughRating,
                    // If the user is logged in, get the rating for the operation, default value is None
                    // If not, set to null
                    RatingType = isLoggedIn
                        ? rating.FirstOrDefault(y => y.OperationId == x.EntityId)?
                              .RatingType ?? OperationRatingType.None
                        : null,
                    Difficulty = x.Difficulty,
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
