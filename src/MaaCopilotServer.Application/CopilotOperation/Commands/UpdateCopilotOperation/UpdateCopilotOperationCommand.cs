// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.UpdateCopilotOperation;

/// <summary>
///     The record of updating a copilot operation.
/// </summary>
[Authorized(UserRole.Uploader)]
public record UpdateCopilotOperationCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The operation content.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }

    /// <summary>
    ///     The operation id.
    /// </summary>
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
        var operationId = _copilotIdService.DecodeId(request.Id!);
        var user = (await _currentUserService.GetUser()).IsNotNull();
        var operation = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.Id == operationId, cancellationToken);

        if (operation is null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.CopilotOperationWithIdNotFound!, request.Id!));
        }

        if (user.IsAllowAccess(operation.Author))
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied!);
        }

        var content = MaaCopilotOperationHelper.DeserializeMaaCopilotOperation(request.Content!).IsNotNull();

        // Parse stage_name and version.
        var stageName = content.GetStageName() ;
        var minimumRequired = content.GetMinimumRequired();

        // Parse doc.
        var docTitle = content.GetDocTitle();
        var docDetails = content.GetDocDetails();

        var operatorArray = content.Operators ?? Array.Empty<MaaCopilotOperationOperator>();
        if (operatorArray.Any(item => item.Name is null))
        {
            return MaaApiResponseHelper.BadRequest(_validationErrorMessage.CopilotOperationJsonIsInvalid);
        }

        var groups = content.SerializeGroup();
        var operators = content.SerializeOperator();

        operation.UpdateOperation(request.Content!, stageName, minimumRequired, docTitle, docDetails, operators, groups, user.EntityId);
        _dbContext.CopilotOperations.Update(operation);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok();
    }
}
