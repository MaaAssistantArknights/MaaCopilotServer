// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

/// <summary>
///     The record of getting user.
/// </summary>
public record GetCopilotUserQuery : IRequest<MaaApiResponse<GetCopilotUserDto>>
{
    /// <summary>
    ///     The user ID.
    /// </summary>
    public string? UserId { get; set; }
}

/// <summary>
///     The handler of getting user.
/// </summary>
public class GetCopilotUserQueryHandler : IRequestHandler<GetCopilotUserQuery, MaaApiResponse<GetCopilotUserDto>>
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage;

    /// <summary>
    ///     The service for current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext;

    /// <summary>
    ///     The constructor of <see cref="GetCopilotUserQueryHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public GetCopilotUserQueryHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    ///     Handles a request of getting user.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with user info of the user.</returns>
    /// <exception cref="PipelineException">Thrown when the current user ID or the user ID to get is not found.</exception>
    public async Task<MaaApiResponse<GetCopilotUserDto>> Handle(GetCopilotUserQuery request,
        CancellationToken cancellationToken)
    {
        Guid userId;
        if (request.UserId == "me")
        {
            var id = _currentUserService.GetUserIdentity();
            if (id is null)
            {
                return MaaApiResponseHelper.BadRequest<GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                    _apiErrorMessage.MeNotFound);
            }

            userId = id.Value;
        }
        else
        {
            userId = Guid.Parse(request.UserId!);
        }

        var user = await _dbContext.CopilotUsers
            .Include(x => x.UserFavorites)
            .FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);

        if (user is null)
        {
            return (MaaApiResponseHelper.NotFound<Common.Models.GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, request.UserId)));
        }

        var uploadCount = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Where(x => x.Author.EntityId == userId)
            .CountAsync(cancellationToken);

        var favList = user.UserFavorites
            .ToDictionary(fav => fav.EntityId.ToString(), fav => fav.FavoriteName);

        var dto = new GetCopilotUserDto(userId, user.UserName, user.UserRole, uploadCount, user.UserActivated, favList);
        return MaaApiResponseHelper.Ok(dto, _currentUserService.GetTrackingId());
    }
}
