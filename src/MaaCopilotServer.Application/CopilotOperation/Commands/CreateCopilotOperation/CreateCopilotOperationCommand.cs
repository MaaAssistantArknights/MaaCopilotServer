// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
///     The record of creating operation.
/// </summary>
[Authorized(UserRole.Uploader)]
public record CreateCopilotOperationCommand : IRequest<MaaActionResult<CreateCopilotOperationDto>>
{
    /// <summary>
    ///     The operation content.
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

/// <summary>
///     The handler of creating operation.
/// </summary>
public class CreateCopilotOperationCommandHandler : IRequestHandler<CreateCopilotOperationCommand,
    MaaActionResult<CreateCopilotOperationDto>>
{
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
    ///     The service for user Identity.
    /// </summary>
    private readonly IIdentityService _identityService;

    private readonly ValidationErrorMessage _validationErrorMessage;

    /// <summary>
    ///     The constructor of <see cref="CreateCopilotOperationCommandHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="identityService"> The service for user Identity.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="copilotIdService">The service for processing copilot ID.</param>
    public CreateCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        IIdentityService identityService,
        ICurrentUserService currentUserService,
        ICopilotIdService copilotIdService,
        ValidationErrorMessage validationErrorMessage)
    {
        _dbContext = dbContext;
        _identityService = identityService;
        _currentUserService = currentUserService;
        _copilotIdService = copilotIdService;
        _validationErrorMessage = validationErrorMessage;
    }

    /// <summary>
    ///     Handles a request of creating operation.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with the response.</returns>
    public async Task<MaaActionResult<CreateCopilotOperationDto>> Handle(CreateCopilotOperationCommand request,
        CancellationToken cancellationToken)
    {
        var doc = JsonDocument.Parse(request.Content!).RootElement;

        // Parse stage_name and version.
        var stageName = doc.GetProperty("stage_name").GetString();
        var minimumRequired = doc.GetProperty("minimum_required").GetString();

        // Parse doc.
        var docTitle = string.Empty;
        var docDetails = string.Empty;
        var hasDoc = doc.TryGetProperty("doc", out var docElement);
        var hasOperator = doc.TryGetProperty("opers", out var operatorElement);
        if (hasDoc && docElement.ValueKind != JsonValueKind.Null)
        {
            var docTitleElementExist = docElement.TryGetProperty("title", out var titleElement);
            var docDetailsElementExist = docElement.TryGetProperty("details", out var detailsElement);
            if (docTitleElementExist)
            {
                docTitle = titleElement.GetString() ?? string.Empty;
            }

            if (docDetailsElementExist)
            {
                docDetails = detailsElement.GetString() ?? string.Empty;
            }
        }

        var operators = new List<string>();
        if (hasOperator && operatorElement.ValueKind != JsonValueKind.Null)
        {
            var operatorElementList = operatorElement.EnumerateArray();
            foreach (var operatorElementItem in operatorElementList)
            {
                var hasName = operatorElementItem.TryGetProperty("name", out var operatorNameElement);
                var hasSkill = operatorElementItem.TryGetProperty("skill", out var operatorSkillElement);
                if (hasName is false || operatorNameElement.ValueKind == JsonValueKind.Null)
                {
                    throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(),
                        _validationErrorMessage.CopilotOperationJsonIsInvalid));
                }

                var operatorItem =
                    $"{operatorNameElement.GetString()}::{(hasSkill ? operatorSkillElement.GetInt32().ToString() : "1")}";
                operators.Add(operatorItem);
            }
        }

        operators = operators.Distinct().ToList();

        var user = await _identityService.GetUserAsync(_currentUserService.GetUserIdentity()!.Value);
        var entity = new Domain.Entities.CopilotOperation(
            request.Content!, stageName!, minimumRequired!, docTitle, docDetails, user!, user!.EntityId, operators);

        _dbContext.CopilotOperations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var id = _copilotIdService.EncodeId(entity.Id);
        return MaaApiResponse.Ok(new CreateCopilotOperationDto(id), _currentUserService.GetTrackingId());
    }
}
