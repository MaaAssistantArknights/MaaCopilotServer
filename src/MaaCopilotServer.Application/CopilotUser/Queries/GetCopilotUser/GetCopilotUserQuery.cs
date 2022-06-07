// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

public record GetCopilotUserQuery : IRequest<MaaActionResult<GetCopilotUserDto>>
{
    public string? UserId { get; set; }
}

public class GetCopilotUserQueryHandler : IRequestHandler<GetCopilotUserQuery, MaaActionResult<GetCopilotUserDto>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetCopilotUserQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<GetCopilotUserDto>> Handle(GetCopilotUserQuery request, CancellationToken cancellationToken)
    {
        Guid userId;
        if (request.UserId == "me")
        {
            var id = _currentUserService.GetUserIdentity();
            if (id is null)
            {
                return MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(), "User is not authenticated.");
            }
            userId = id.Value;
        }
        else
        {
            userId = Guid.Parse(request.UserId!);
        }
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);

        if (user is null)
        {
            return MaaApiResponse.NotFound($"User with id {request.UserId}", _currentUserService.GetTrackingId());
        }

        var uploadCount = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Where(x => x.Author.EntityId == userId)
            .CountAsync(cancellationToken);

        var dto = new GetCopilotUserDto(userId, user.UserName, user.UserRole, uploadCount);
        return MaaApiResponse.Ok(dto, _currentUserService.GetTrackingId());
    }
}
