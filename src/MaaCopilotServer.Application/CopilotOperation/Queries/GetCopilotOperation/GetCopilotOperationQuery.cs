// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;

public record GetCopilotOperationQuery : IRequest<MaaActionResult<GetCopilotOperationQueryDto>>
{
    public string? Id { get; set; }
}

public class GetCopilotOperationQueryHandler : IRequestHandler<GetCopilotOperationQuery, MaaActionResult<GetCopilotOperationQueryDto>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICopilotIdService _copilotIdService;
    private readonly ICurrentUserService _currentUserService;

    public GetCopilotOperationQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICopilotIdService copilotIdService,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _copilotIdService = copilotIdService;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<GetCopilotOperationQueryDto>> Handle(GetCopilotOperationQuery request, CancellationToken cancellationToken)
    {
        var entityId = _copilotIdService.GetEntityId(request.Id!);
        if (entityId is null)
        {
            return MaaApiResponse.NotFound("CopilotOperation", _currentUserService.GetTrackingId());
        }

        var entity = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .FirstOrDefaultAsync(x => x.EntityId == entityId.Value, cancellationToken);
        if (entity is null)
        {
            return MaaApiResponse.NotFound("CopilotOperation", _currentUserService.GetTrackingId());
        }

        var dto = new GetCopilotOperationQueryDto(
            _copilotIdService.GetCopilotId(entity.EntityId), entity.StageName, entity.MinimumRequired,
            entity.CreateAt.ToStringZhHans(), entity.Content, entity.Author.UserName);
        return MaaApiResponse.Ok(dto, _currentUserService.GetTrackingId());
    }
}
