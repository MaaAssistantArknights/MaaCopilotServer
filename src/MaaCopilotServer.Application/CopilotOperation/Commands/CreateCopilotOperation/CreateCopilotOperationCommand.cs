// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;

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
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ValidationErrorMessage _validationErrorMessage;

    public CreateCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ICopilotIdService copilotIdService,
        ValidationErrorMessage validationErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _copilotIdService = copilotIdService;
        _validationErrorMessage = validationErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(CreateCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        var content = MaaCopilotOperationHelper.DeserializeMaaCopilotOperation(request.Content!).IsNotNull();

        // Parse stage_name and version.
        var stageName = content.GetStageName();
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

        var user = await _currentUserService.GetUser();
        var entity = new Domain.Entities.CopilotOperation(
            request.Content!, stageName!, minimumRequired!, docTitle, docDetails, user!, user!.EntityId, operators, groups);

        _dbContext.CopilotOperations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var id = _copilotIdService.EncodeId(entity.Id);
        return MaaApiResponseHelper.Ok(new CreateCopilotOperationDto()
        {
            Id = id,
        });
    }
}
