// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

public record GetCopilotUserQuery : IRequest<MaaActionResult<GetCopilotUserDto>>
{
    public string? UserId { get; set; }
}

public class GetCopilotUserQueryHandler : IRequestHandler<GetCopilotUserQuery, MaaActionResult<GetCopilotUserDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;

    public GetCopilotUserQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaActionResult<GetCopilotUserDto>> Handle(GetCopilotUserQuery request,
        CancellationToken cancellationToken)
    {
        Guid userId;
        if (request.UserId == "me")
        {
            var id = _currentUserService.GetUserIdentity();
            if (id is null)
            {
                throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(),
                    _apiErrorMessage.MeNotFound));
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
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, request.UserId)));
        }

        var uploadCount = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Where(x => x.Author.EntityId == userId)
            .CountAsync(cancellationToken);

        var dto = new GetCopilotUserDto(userId, user.UserName, user.UserRole, uploadCount);
        return MaaApiResponse.Ok(dto, _currentUserService.GetTrackingId());
    }
}
