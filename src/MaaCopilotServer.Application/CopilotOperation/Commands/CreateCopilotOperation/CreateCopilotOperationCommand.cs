// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.Common.Operation;
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
    private readonly ICopilotOperationService _copilotOperationService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IOperationProcessService _operationProcessService;
    private readonly IMaaCopilotDbContext _dbContext;

    public CreateCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        IOperationProcessService operationProcessService,
        ICopilotOperationService copilotOperationService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _operationProcessService = operationProcessService;
        _copilotOperationService = copilotOperationService;
    }

    public async Task<MaaApiResponse> Handle(CreateCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        // Validate operation JSON content.
        var validationResult = await _operationProcessService.Validate(request.Content);

        if (validationResult.IsValid is false)
        {
            return MaaApiResponseHelper.BadRequest(validationResult.ErrorMessages);
        }

        var obj = validationResult.Operation!;

        // Get user
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Check if the stage is custom stage or not
        var realContent = validationResult.ArkLevel!.Custom
            ? request.Content!.Replace(
                validationResult.ArkLevel.LevelId,
                validationResult.ArkLevel.LevelId.Replace("copilot-custom/", string.Empty))
            : request.Content!;

        // Build entity
        var entity = new Domain.Entities.CopilotOperation(
            realContent,
            obj.MinimumRequired!,
            obj.Doc!.Title!,
            obj.Doc!.Details!,
            user,
            user.EntityId,
            validationResult.ArkLevel!,
            obj.SerializeOperator(),
            obj.SerializeGroup(),
            obj.Difficulty ?? DifficultyType.Unknown);
        entity.UpdateHotScore(_copilotOperationService.CalculateHotScore(entity));

        // Add entity to database.
        _dbContext.CopilotOperations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Build response.
        var id = EntityIdHelper.EncodeId(entity.Id);
        return MaaApiResponseHelper.Ok(new CreateCopilotOperationDto()
        {
            Id = id,
        });
    }
}
