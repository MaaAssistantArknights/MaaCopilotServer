// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

/// <summary>
///     The record of creating operation.
/// </summary>
[Authorized(UserRole.Uploader)]
public record CreateCopilotOperationCommand : IRequest<MaaApiResponse>
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
public class CreateCopilotOperationCommandHandler : IRequestHandler<CreateCopilotOperationCommand, MaaApiResponse>
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

    private readonly ValidationErrorMessage _validationErrorMessage;

    /// <summary>
    ///     The constructor of <see cref="CreateCopilotOperationCommandHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="copilotIdService">The service for processing copilot ID.</param>
    /// <param name="validationErrorMessage">The resource of validation error messages.</param>
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

    /// <summary>
    ///     Handles a request of creating operation.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with the response.</returns>
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
