// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

/// <summary>
///     The record of querying operation.
/// </summary>
public record GetCopilotOperationQuery : IRequest<MaaActionResult<GetCopilotOperationQueryDto>>
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
        MaaActionResult<GetCopilotOperationQueryDto>>
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
    ///     The constructor of <see cref="GetCopilotOperationQueryHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="copilotIdService">The service for processing copilot ID.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public GetCopilotOperationQueryHandler(
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
    ///     Handles a request of querying operation.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with a single operation and info.</returns>
    /// <exception cref="PipelineException">Thrown when the operation ID is invalid or not found.</exception>
    public async Task<MaaActionResult<GetCopilotOperationQueryDto>> Handle(GetCopilotOperationQuery request,
        CancellationToken cancellationToken)
    {
        var id = _copilotIdService.DecodeId(request.Id!);
        if (id is null)
        {
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id)));
        }

        var entity = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        if (entity is null)
        {
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id)));
        }

        var dto = new GetCopilotOperationQueryDto(
            request.Id!, entity.StageName, entity.MinimumRequired,
            entity.CreateAt.ToIsoString()
            , entity.Content, entity.Author.UserName, entity.Title, entity.Details, entity.Downloads, entity.Operators);

        entity.AddDownloadCount();
        _dbContext.CopilotOperations.Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponse.Ok(dto, _currentUserService.GetTrackingId());
    }
}
