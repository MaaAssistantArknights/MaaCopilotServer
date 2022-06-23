// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;

/// <summary>
///     The record of deleting operation.
/// </summary>
[Authorized(UserRole.Admin)]
public record DeleteCopilotOperationCommand : IRequest<MaaApiResponse<EmptyObject>>
{
    /// <summary>
    ///     The operation ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

/// <summary>
///     The handler of deleting operation.
/// </summary>
public class DeleteCopilotOperationCommandHandler : IRequestHandler<DeleteCopilotOperationCommand,
    MaaApiResponse<EmptyObject>>
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
    ///     The constructor of <see cref="DeleteCopilotOperationCommandHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="copilotIdService">The service for processing copilot ID.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public DeleteCopilotOperationCommandHandler(
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
    ///     Handles a request of deleting operation.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with the response.</returns>
    /// <exception cref="PipelineException">
    ///     Thrown when the operation ID is invalid, or the operation does not exist.
    /// </exception>
    public async Task<MaaApiResponse<EmptyObject>> Handle(DeleteCopilotOperationCommand request,
        CancellationToken cancellationToken)
    {
        var id = _copilotIdService.DecodeId(request.Id!);
        if (id is null)
        {
            throw new PipelineException(MaaApiResponseHelper.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id)));
        }

        var entity = await _dbContext.CopilotOperations.FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        if (entity is null)
        {
            throw new PipelineException(MaaApiResponseHelper.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id)));
        }

        entity.Delete(_currentUserService.GetUserIdentity()!.Value);
        _dbContext.CopilotOperations.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok<EmptyObject>(null, _currentUserService.GetTrackingId());
    }
}
