// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
///     The DTO for the CreateCopilotOperation command.
/// </summary>
[Authorized(UserRole.Uploader)]
public record CreateCopilotOperationCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The operation JSON content.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

public class CreateCopilotOperationCommandHandler : IRequestHandler<CreateCopilotOperationCommand, MaaApiResponse>
{
    private readonly ICopilotOperationService _copilotOperationService;
    private readonly IOptions<CopilotOperationOption> _copilotOperationOption;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ValidationErrorMessage _validationErrorMessage;

    public CreateCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ICopilotOperationService copilotOperationService,
        IOptions<CopilotOperationOption> copilotOperationOption,
        ValidationErrorMessage validationErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _copilotOperationService = copilotOperationService;
        _copilotOperationOption = copilotOperationOption;
        _validationErrorMessage = validationErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(CreateCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        // Deserialize the operation JSON content.
        var content = MaaCopilotOperationHelper.DeserializeMaaCopilotOperation(request.Content!).IsNotNull();

        // Parse stage_name and version.
        var stageName = content.GetStageName();
        var minimumRequired = content.GetMinimumRequired();

        // Parse doc.
        var docTitle = content.GetDocTitle();
        var docDetails = content.GetDocDetails();

        // Check configuration if title and details are required.
        if (_copilotOperationOption.Value.RequireDetails && string.IsNullOrEmpty(docTitle))
        {
            return MaaApiResponseHelper.BadRequest(_validationErrorMessage.CopilotOperationJsonIsInvalid);
        }
        if (_copilotOperationOption.Value.RequireDetails && string.IsNullOrEmpty(docDetails))
        {
            return MaaApiResponseHelper.BadRequest(_validationErrorMessage.CopilotOperationJsonIsInvalid);
        }

        // Check operators
        var operatorArray = content.Operators ?? Array.Empty<MaaCopilotOperationOperator>();
        if (operatorArray.Any(item => item.Name is null))
        {
            return MaaApiResponseHelper.BadRequest(_validationErrorMessage.CopilotOperationJsonIsInvalid);
        }

        // Get groups and operators
        var groups = content.SerializeGroup();
        var operators = content.SerializeOperator();

        // Get user
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Build entity
        var entity = new Domain.Entities.CopilotOperation(
            request.Content!, stageName, minimumRequired, docTitle, docDetails, user, user.EntityId, operators, groups);
        entity.UpdateHotScore(_copilotOperationService.CalculateHotScore(entity));

        // Add entity to database.
        _dbContext.CopilotOperations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Build response.
        var id = _copilotOperationService.EncodeId(entity.Id);
        return MaaApiResponseHelper.Ok(new CreateCopilotOperationDto()
        {
            Id = id,
        });
    }
}
