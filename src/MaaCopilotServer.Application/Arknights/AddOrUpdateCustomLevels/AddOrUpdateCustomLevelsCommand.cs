// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.GameData;
using Microsoft.EntityFrameworkCore;
using ArkI18N = MaaCopilotServer.Domain.Entities.ArkI18N;

namespace MaaCopilotServer.Application.Arknights.AddOrUpdateCustomLevels;

public record AddOrUpdateCustomLevelsCommand
{
    /// <summary>
    ///     Arknights level name.
    /// </summary>
    [JsonPropertyName("name")]
    public ArkI18NDto Name { get; set; } = new();
    
    /// <summary>
    ///     Arknights level category one. Typically zone name.
    /// </summary>
    [JsonPropertyName("cat_one")]
    public ArkI18NDto CatOne { get; set; } = new();
    
    /// <summary>
    ///     Arknights level category two. Typically chapter name.
    /// </summary>
    [JsonPropertyName("cat_two")]
    public ArkI18NDto CatTwo { get; set; } = new();
    
    /// <summary>
    ///     Arknights level category three. Typically level code.
    /// </summary>
    [JsonPropertyName("cat_three")]
    public ArkI18NDto CatThree { get; set; } = new();

    /// <summary>
    ///     Arknights level id. Custom levels will have `copilot-custom/` prefix added to their id automatically.
    /// </summary>
    [JsonPropertyName("level_id")]
    public string LevelId { get; set; } = string.Empty;

    /// <summary>
    ///     Map width, defaults to 0.
    /// </summary>
    [JsonPropertyName("width")]
    public int Width { get; set; } = 0;

    /// <summary>
    ///     Map height, defaults to 0.
    /// </summary>
    [JsonPropertyName("height")]
    public int Height { get; set; } = 0;
}

[Authorized(UserRole.Admin)]
public record AddOrUpdateCustomLevelsCommandBatch : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     Custom levels to add or update.
    /// </summary>
    [JsonPropertyName("custom_levels")]
    // ReSharper disable once CollectionNeverUpdated.Global
    public List<AddOrUpdateCustomLevelsCommand> Commands { get; set; } = new();
}

public class  AddOrUpdateCustomLevelsCommandBatchHandler : IRequestHandler<AddOrUpdateCustomLevelsCommandBatch, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public AddOrUpdateCustomLevelsCommandBatchHandler(IMaaCopilotDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<MaaApiResponse> Handle(AddOrUpdateCustomLevelsCommandBatch request, CancellationToken cancellationToken)
    {
        var uid = _currentUserService.GetUserIdentity();
        if (uid is null)
        {
            return MaaApiResponseHelper.InternalError();
        }
        
        var dbE = _dbContext.ArkLevelData
            .Include(x => x.Name)
            .Include(x => x.CatOne)
            .Include(x => x.CatTwo)
            .Include(x => x.CatThree)
            .Where(x => x.Custom == true)
            .ToList();

        var pending = request.Commands.Select(x =>
                new ArkLevelData(uid.Value)
                {
                    Name = x.Name.ToDomainEntityFromDto(),
                    CatOne = x.CatOne.ToDomainEntityFromDto(),
                    CatTwo = x.CatTwo.ToDomainEntityFromDto(),
                    CatThree = x.CatThree.ToDomainEntityFromDto(),
                    Height = x.Height,
                    Width = x.Width,
                    Custom = true,
                    LevelId = $"copilot-custom/{x.LevelId}",
                    Keyword = Helpers.BuildKeyword(
                            x.Name.ToGameDataEntityFromDto(),
                            x.CatOne.ToGameDataEntityFromDto(),
                            x.CatTwo.ToGameDataEntityFromDto(),
                            x.CatThree.ToGameDataEntityFromDto())
                        .ToDomainEntityFromGameDataEntity()
                })
            .ToList();

        var added = new List<string>();
        var updated = new List<string>();
        
        foreach (var command in pending)
        {
            var exist = dbE.FirstOrDefault(x => x.LevelId == command.LevelId);
            if (exist is null)
            {
                await _dbContext.ArkLevelData.AddAsync(command, cancellationToken);
                added.Add(command.LevelId);
                continue;
            }

            exist.Name.Update(command.Name.ToGameDataEntityFromDomainEntity());
            exist.CatOne.Update(command.CatOne.ToGameDataEntityFromDomainEntity());
            exist.CatTwo.Update(command.CatTwo.ToGameDataEntityFromDomainEntity());
            exist.CatThree.Update(command.CatThree.ToGameDataEntityFromDomainEntity());

            exist.Keyword ??= new ArkI18N();
            exist.Keyword.Update(command.Keyword!.ToGameDataEntityFromDomainEntity());

            exist.LevelId = command.LevelId;
            exist.Width = command.Width;
            exist.Height = command.Height;

            _dbContext.ArkLevelData.Update(exist);
            updated.Add(command.LevelId);
        }

        var changes = await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok(new AddOrUpdateCustomLevelsDto
        {
            DbContextChanges = changes, Added = added, Updated = updated
        });
    }
}
