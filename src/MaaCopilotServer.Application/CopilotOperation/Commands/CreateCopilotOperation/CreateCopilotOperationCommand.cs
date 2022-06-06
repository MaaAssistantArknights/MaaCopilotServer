// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Security;
using MaaCopilotServer.Domain.Enums;
using MediatR;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;

public record CreateCopilotOperationCommand : IRequest<MaaActionResult<CreateCopilotOperationDto>>
{
    public string? Content { get; set; }
}

[Authorized(UserRole.Uploader)]
public class CreateCopilotOperationCommandHandler : IRequestHandler<CreateCopilotOperationCommand, MaaActionResult<CreateCopilotOperationDto>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ICopilotIdService _copilotIdService;

    public CreateCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        IIdentityService identityService,
        ICurrentUserService currentUserService,
        ICopilotIdService copilotIdService)
    {
        _dbContext = dbContext;
        _identityService = identityService;
        _currentUserService = currentUserService;
        _copilotIdService = copilotIdService;
    }

    public async Task<MaaActionResult<CreateCopilotOperationDto>> Handle(CreateCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        var doc = JsonDocument.Parse(request.Content!).RootElement;

        var stageName = doc.GetProperty("stage_name").GetString();
        var minimumRequired = doc.GetProperty("minimum_required").GetString();

        var docTitle = string.Empty;
        var docDetails = string.Empty;
        var hasDoc = doc.TryGetProperty("doc", out var docElement);
        if (hasDoc)
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

        var user = await _identityService.GetUserAsync(_currentUserService.GetUserIdentity()!.Value);
        var entity = new Domain.Entities.CopilotOperation(
            request.Content!, stageName!, minimumRequired!, docTitle, docDetails, user!);

        _dbContext.CopilotOperations.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var id = _copilotIdService.GetCopilotId(entity.EntityId);
        return MaaApiResponse.Ok(new CreateCopilotOperationDto(id), _currentUserService.GetTrackingId());
    }
}
