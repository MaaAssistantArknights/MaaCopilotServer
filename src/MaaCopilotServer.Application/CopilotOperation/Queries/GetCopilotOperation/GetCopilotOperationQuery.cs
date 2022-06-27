// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Helper;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
///     The record of querying operation.
/// </summary>
public record GetCopilotOperationQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The operation ID.
    /// </summary>
    public string? Id { get; set; }
}

/// <summary>
///     The handler of querying operation.
/// </summary>
public class
    GetCopilotOperationQueryHandler : IRequestHandler<GetCopilotOperationQuery,
        MaaApiResponse>
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
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext;

    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The constructor of <see cref="GetCopilotOperationQueryHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="currentUserService">The current user service.</param>
    /// <param name="copilotIdService">The service for processing copilot ID.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public GetCopilotOperationQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ICopilotIdService copilotIdService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _copilotIdService = copilotIdService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    ///     Handles a request of querying operation.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task with a single operation and info.</para>
    ///     <para>404 when the operation ID is invalid or not found.</para>
    /// </returns>
    public async Task<MaaApiResponse> Handle(GetCopilotOperationQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _currentUserService.GetUser();
        var isLoggedIn = user is not null;
        var id = _copilotIdService.DecodeId(request.Id!);
        if (id is null)
        {
            return MaaApiResponseHelper.NotFound(string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id));
        }

        var entity = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        if (entity is null)
        {
            return MaaApiResponseHelper.NotFound(string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id));
        }

        OperationRatingType? rating = isLoggedIn
            ? (await _dbContext.CopilotOperationRatings
                .FirstOrDefaultAsync(x => x.UserId == user!.EntityId
                                          && x.OperationId == entity.EntityId, cancellationToken))?
                .RatingType ?? OperationRatingType.None
            : null;

        var dto = new GetCopilotOperationQueryDto
        {
            Id = request.Id!,
            StageName = entity.StageName,
            MinimumRequired = entity.MinimumRequired,
            Content = entity.Content,
            Detail = entity.Details,
            Operators = entity.Operators,
            Title = entity.Title,
            Uploader = entity.Author.UserName,
            UploadTime = entity.CreateAt.ToIsoString(),
            ViewCounts = entity.ViewCounts,
            RatingRatio = entity.RatingRatio,
            Groups = entity.Groups.ToArray().DeserializeGroup(),
            RatingType = rating
        };

        entity.AddViewCount();
        _dbContext.CopilotOperations.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok(dto);
    }
}
