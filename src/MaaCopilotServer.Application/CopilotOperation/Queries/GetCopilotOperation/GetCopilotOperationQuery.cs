// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
///     The DTO for the GetCopilotOperation query.
/// </summary>
public record GetCopilotOperationQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The operation id.
    /// </summary>
    [Required]
    public string? Id { get; set; }
}

public class
    GetCopilotOperationQueryHandler : IRequestHandler<GetCopilotOperationQuery,
        MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICopilotIdService _copilotIdService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

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
