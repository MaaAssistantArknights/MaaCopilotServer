// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotFavorite.Commands.AddFavorite;
using MaaCopilotServer.Application.CopilotFavorite.Commands.CreateFavoriteList;
using MaaCopilotServer.Application.CopilotFavorite.Commands.DeleteFavoriteList;
using MaaCopilotServer.Application.CopilotFavorite.Commands.RemoveFavorite;
using MaaCopilotServer.Application.CopilotFavorite.Queries.GetCopilotUserFavorites;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

[ApiController]
[Route("favorites")]
public class CopilotFavoriteController : MaaControllerBase
{
    public CopilotFavoriteController(IMediator mediator) : base(mediator) { }

    [HttpPost("create")]
    public async Task<ActionResult> CreateFavoriteList([FromBody] CreateFavoriteListCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteFavoriteList([FromBody] DeleteFavoriteListCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("add")]
    public async Task<ActionResult> AddFavoriteToList([FromBody] AddFavoriteCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("remove")]
    public async Task<ActionResult> RemoveFavoriteFromList([FromBody] RemoveFavoriteCommand command)
    {
        return await GetResponse(command);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult> GetFavoriteList(string id)
    {
        var query = new GetCopilotUserFavoritesQuery { FavoriteListId = id };
        return await GetResponse(query);
    }
}
