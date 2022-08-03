// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Operation;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.UpdateCopilotOperation;

/// <summary>
///     The DTO for the UpdateCopilotOperation command.
/// </summary>
[Authorized(UserRole.Uploader)]
public record UpdateCopilotOperationCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The operation JSON content.
    /// </summary>
    [Required]
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>
    ///     The operation id which is pending to change.
    /// </summary>
    [Required]
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class UpdateCopilotOperationCommandHandler : IRequestHandler<UpdateCopilotOperationCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICopilotOperationService _copilotOperationService;
    private readonly IOperationProcessService _operationProcessService;
    private readonly ApiErrorMessage _apiErrorMessage;

    public UpdateCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ICopilotOperationService copilotOperationService,
        IOperationProcessService operationProcessService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _copilotOperationService = copilotOperationService;
        _operationProcessService = operationProcessService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(UpdateCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        // Get current infos
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Get operation
        var operationId = EntityIdHelper.DecodeId(request.Id!);
        var operation = await _dbContext.CopilotOperations
            .Include(x => x.ArkLevel)
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == operationId, cancellationToken);
        if (operation is null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id!));
        }

        // Check if the user has the permission to update the operation
        if (user.IsAllowAccess(operation.Author) is false)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied!);
        }

        // Validate operation JSON content.
        var validationResult = await _operationProcessService.Validate(request.Content);

        if (validationResult.IsValid is false)
        {
            return MaaApiResponseHelper.BadRequest(validationResult.ErrorMessages);
        }

        var obj = validationResult.Operation!;

        // Check if level is the same as the one in the database.
        if (validationResult.ArkLevel!.LevelId != operation.ArkLevel.LevelId)
        {
            return MaaApiResponseHelper.BadRequest();
        }

        // Update the operation
        operation.UpdateOperation(
            request.Content!,
            obj.MinimumRequired!,
            obj.Doc!.Title!,
            obj.Doc!.Details!,
            obj.SerializeOperator(),
            obj.SerializeGroup(),
            user.EntityId);
        _dbContext.CopilotOperations.Update(operation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok();
    }
}
