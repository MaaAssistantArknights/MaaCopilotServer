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
    /// <param name="validationErrorMessage">The resource of validation error messages.</param>
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
    public async Task<MaaApiResponse> Handle(CreateCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        var content = JsonSerializer.Deserialize<CreateCopilotOperationContent>(request.Content!).IsNotNull();

        // Parse stage_name and version.
        var stageName = content.StageName;
        var minimumRequired = content.MinimumRequired;

        // Parse doc.
        var docTitle = content.Doc?.Title ?? string.Empty;
        var docDetails = content.Doc?.Details ?? string.Empty;

        var operatorArray = content.Operators ?? Array.Empty<Operator>();
        if (operatorArray.Any(item => item.Name == null))
        {
            return MaaApiResponseHelper.BadRequest(_validationErrorMessage.CopilotOperationJsonIsInvalid);
        }

        var operators = (content.Operators ?? Array.Empty<Operator>())
            .Select(item => $"{item.Name}::{item.Skill ?? 1}")
            .Distinct() // Remove duplicate operators.
            .ToList();

        var user = await _identityService.GetUserAsync(_currentUserService.GetUserIdentity()!.Value);
        var entity = new Domain.Entities.CopilotOperation(
            request.Content!, stageName!, minimumRequired!, docTitle, docDetails, user!, user!.EntityId, operators);

        _dbContext.CopilotOperations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var id = _copilotIdService.EncodeId(entity.Id);
        return MaaApiResponseHelper.Ok(new CreateCopilotOperationDto(id));
    }
}

/// <summary>
/// The JSON request content of creating copilot operation.
/// </summary>
internal record CreateCopilotOperationContent
{
    /// <summary>
    /// The <c>stage_name</c> field.
    /// </summary>
    [JsonPropertyName("stage_name")]
    public string? StageName { get; set; }

    /// <summary>
    /// The <c>minimum_required</c> field.
    /// </summary>
    [JsonPropertyName("minimum_required")]
    public string? MinimumRequired { get; set; }

    /// <summary>
    /// The <c>doc</c> field.
    /// </summary>
    [JsonPropertyName("doc")]
    public Doc? Doc { get; set; }

    /// <summary>
    /// The <c>opers</c> field.
    /// </summary>
    [JsonPropertyName("opers")]
    public Operator[]? Operators { get; set; }
}

/// <summary>
/// The JSON content of <c>doc</c>.
/// </summary>
internal record Doc
{
    /// <summary>
    /// The <c>title</c> field.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// The <c>details</c> field.
    /// </summary>
    [JsonPropertyName("details")]
    public string? Details { get; set; }
}

/// <summary>
/// The JSON content of <c>operator</c>.
/// </summary>
internal record Operator
{
    /// <summary>
    /// The <c>name</c> field.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The <c>skill</c> field.
    /// </summary>
    [JsonPropertyName("skill")]
    public int? Skill { get; set; }
}
