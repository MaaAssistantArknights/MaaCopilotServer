// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
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
    private readonly ICopilotIdService _copilotIdService;
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ValidationErrorMessage _validationErrorMessage;

    public UpdateCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ICopilotIdService copilotIdService,
        ApiErrorMessage apiErrorMessage,
        ValidationErrorMessage validationErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _copilotIdService = copilotIdService;
        _apiErrorMessage = apiErrorMessage;
        _validationErrorMessage = validationErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(UpdateCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        // Get current infos
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Get operation
        var operationId = _copilotIdService.DecodeId(request.Id!);
        var operation = await _dbContext.CopilotOperations
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

        // Deserialize the operation JSON content
        var content = MaaCopilotOperationHelper.DeserializeMaaCopilotOperation(request.Content!).IsNotNull();

        // Parse stage_name and version.
        var stageName = content.GetStageName() ;
        var minimumRequired = content.GetMinimumRequired();

        // Parse doc.
        var docTitle = content.GetDocTitle();
        var docDetails = content.GetDocDetails();

        // Check if the operator field is valid
        var operatorArray = content.Operators ?? Array.Empty<MaaCopilotOperationOperator>();
        if (operatorArray.Any(item => item.Name is null))
        {
            return MaaApiResponseHelper.BadRequest(_validationErrorMessage.CopilotOperationJsonIsInvalid);
        }

        // Parse groups and operators
        var groups = content.SerializeGroup();
        var operators = content.SerializeOperator();

        // Update the operation
        operation.UpdateOperation(request.Content!, stageName, minimumRequired, docTitle, docDetails, operators, groups, user.EntityId);
        _dbContext.CopilotOperations.Update(operation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok();
    }
}
