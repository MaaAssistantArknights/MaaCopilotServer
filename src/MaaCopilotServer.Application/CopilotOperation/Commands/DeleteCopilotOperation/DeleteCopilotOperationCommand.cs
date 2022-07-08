// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;

/// <summary>
///     The DTO for the DeleteCopilotOperation command.
/// </summary>
[Authorized(UserRole.Uploader)]
public record DeleteCopilotOperationCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The operation ID.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class DeleteCopilotOperationCommandHandler : IRequestHandler<DeleteCopilotOperationCommand,
    MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICopilotOperationService _copilotOperationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public DeleteCopilotOperationCommandHandler(
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

    public async Task<MaaApiResponse> Handle(DeleteCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        // Get current infos
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Get operation
        var id = _copilotOperationService.DecodeId(request.Id!);
        var entity = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
        {
            return MaaApiResponseHelper.NotFound(string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id));
        }

        // Check if the user has the permission to delete the operation
        if (user.IsAllowAccess(entity.Author) is false)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied);
        }

        // Delete operation
        entity.Delete(user.EntityId);
        _dbContext.CopilotOperations.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
