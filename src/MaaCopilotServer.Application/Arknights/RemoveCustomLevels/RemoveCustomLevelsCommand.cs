// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Arknights.RemoveCustomLevels;

[Authorized(UserRole.SuperAdmin)]
public record RemoveCustomLevelsCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     A list of level ids that is pending to be removed.
    /// </summary>
    [JsonPropertyName("level_ids")]
    public List<string> LevelIds { get; set; } = new();
}

public class RemoveCustomLevelsCommandHandler : IRequestHandler<RemoveCustomLevelsCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public RemoveCustomLevelsCommandHandler(IMaaCopilotDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }
    
    public async Task<MaaApiResponse> Handle(RemoveCustomLevelsCommand request, CancellationToken cancellationToken)
    {
        var uid = _currentUserService.GetUserIdentity();
        if (uid is null)
        {
            return MaaApiResponseHelper.InternalError();
        }
        
        var removed = new List<string>();
        
        foreach (var levelId in request.LevelIds)
        {
            var exist = await _dbContext.ArkLevelData
                .Where(x => x.Custom)
                .FirstOrDefaultAsync(x => x.LevelId == $"copilot-custom/{levelId}",
                    cancellationToken: cancellationToken);

            if (exist is null)
            {
                continue;
            }

            exist.Delete(uid.Value);
            _dbContext.ArkLevelData.Remove(exist);
            removed.Add(exist.LevelId);
        }

        var changes = await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok(new RemoveCustomLevelsDto
        {
            DbContextChanges = changes, Removed = removed
        });
    }
}
