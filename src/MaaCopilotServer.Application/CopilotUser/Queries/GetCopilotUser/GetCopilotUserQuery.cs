// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using MaaCopilotServer.Application.Common.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;

/// <summary>
///     The DTO for the GetCopilotUser query.
/// </summary>
public record GetCopilotUserQuery : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user id.
    /// </summary>
    [Required]
    public string? UserId { get; set; }
}

public class GetCopilotUserQueryHandler : IRequestHandler<GetCopilotUserQuery, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
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

    public async Task<MaaApiResponse> Handle(GetCopilotUserQuery request,
        CancellationToken cancellationToken)
    {
        Guid userId;
        if (request.UserId == "me")
        {
            var id = _currentUserService.GetUserIdentity();
            if (id is null)
            {
                return MaaApiResponseHelper.BadRequest(
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
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.UserWithIdNotFound!, request.UserId));
        }

        var uploadCount = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Where(x => x.Author.EntityId == userId)
            .CountAsync(cancellationToken);

        var favList = user.UserFavorites
            .ToDictionary(fav => fav.EntityId.ToString(), fav => fav.FavoriteName);

        var dto = new GetCopilotUserDto(userId, user.UserName, user.UserRole, uploadCount, user.UserActivated, favList);
        return MaaApiResponseHelper.Ok(dto);
    }
}
