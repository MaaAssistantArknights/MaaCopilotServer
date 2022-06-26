// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotFavorite.Commands.CreateFavoriteList;

[Authorized(UserRole.User)]
public record CreateFavoriteListCommand : IRequest<MaaApiResponse>
{
    [JsonPropertyName("favorite_list_name")] public string? Name { get; set; }
}

public class CreateFavoriteListCommandHandler : IRequestHandler<CreateFavoriteListCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateFavoriteListCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<MaaApiResponse> Handle(CreateFavoriteListCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.GetUserIdentity()!.Value;
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);

        if (user is null)
        {
            return MaaApiResponseHelper.InternalError();
        }

        var list = new CopilotUserFavorite(user, request.Name!);
        _dbContext.CopilotUserFavorites.Add(list);
        await _dbContext.SaveChangesAsync(cancellationToken);
        var dto = new CreateFavoriteListDto { Id = list.EntityId.ToString(), Name = list.FavoriteName };
        return MaaApiResponseHelper.Ok(dto);
    }
}
