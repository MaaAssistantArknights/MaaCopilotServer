// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application.CopilotFavorite.Commands.AddFavorite;
using MaaCopilotServer.Application.CopilotFavorite.Commands.CreateFavoriteList;
using MaaCopilotServer.Application.CopilotFavorite.Commands.DeleteFavoriteList;
using MaaCopilotServer.Application.CopilotFavorite.Commands.RemoveFavorite;
using MaaCopilotServer.Application.CopilotFavorite.Queries.GetCopilotUserFavorites;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller of copilot operations under "favorites" endpoint.
/// Include operations related to user favorites and favorite lists.
/// </summary>
/// <response code="400">A bad request, most cases are invalid request parameters.</response>
/// <response code="401">An unauthorized request, you need to login and set Authorization header at first.</response>
/// <response code="403">A forbidden request, you do not have permission to perform the operation.</response>
/// <response code="404">Some thing not found.</response>
/// <response code="500">Some server errors happens.</response>
[ApiController]
[Route("favorites")]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status500InternalServerError)]
public class CopilotFavoriteController : MaaControllerBase
{
    /// <summary>
    ///     The constructor of <see cref="CopilotFavoriteController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public CopilotFavoriteController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     Creates a new favorite list.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The favorite list was created successfully.</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(MaaApiResponseModel<CreateFavoriteListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateFavoriteList([FromBody] CreateFavoriteListCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Deletes a favorite list.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The favorite list was deleted successfully.</response>
    [HttpPost("delete")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteFavoriteList([FromBody] DeleteFavoriteListCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Adds a new favorite to a favorite list.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The favorite was added successfully.</response>
    [HttpPost("add")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> AddFavoriteToList([FromBody] AddFavoriteCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Removes a favorite from a favorite list.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The favorite was removed successfully.</response>
    [HttpPost("remove")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveFavoriteFromList([FromBody] RemoveFavoriteCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Get the user favorites by a specific favorite list id.
    /// </summary>
    /// <param name="id">The favorite list id.</param>
    /// <response code="200">The user favorites in a favorite list.</response>
    [HttpGet("get/{id}")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCopilotUserFavoritesDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetFavoriteList(string id)
    {
        var query = new GetCopilotUserFavoritesQuery { FavoriteListId = id };
        return await GetResponse(query);
    }
}
