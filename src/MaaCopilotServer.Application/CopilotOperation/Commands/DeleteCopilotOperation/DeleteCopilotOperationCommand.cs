// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Security;
using MaaCopilotServer.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;

[Authorized(UserRole.Admin)]
public record DeleteCopilotOperationCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("id")] public string? Id { get; set; }
}

public class DeleteCopilotOperationCommandHandler : IRequestHandler<DeleteCopilotOperationCommand,
    MaaActionResult<EmptyObject>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCopilotOperationCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICopilotIdService copilotIdService,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _copilotIdService = copilotIdService;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<EmptyObject>> Handle(DeleteCopilotOperationCommand request, CancellationToken cancellationToken)
    {
        var id = _copilotIdService.DecodeId(request.Id!);
        if (id is null)
        {
            return MaaApiResponse.NotFound("CopilotOperation", _currentUserService.GetTrackingId());
        }

        var entity = await _dbContext.CopilotOperations.FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);
        if (entity is null)
        {
            return MaaApiResponse.NotFound("CopilotOperation", _currentUserService.GetTrackingId());
        }

        entity.Delete(_currentUserService.GetUserIdentity()!.Value);
        _dbContext.CopilotOperations.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(new EmptyObject(), _currentUserService.GetTrackingId());
    }
}
