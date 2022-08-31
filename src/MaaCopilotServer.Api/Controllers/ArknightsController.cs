// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application.Arknights.AddOrUpdateCustomLevels;
using MaaCopilotServer.Application.Arknights.GetDataVersion;
using MaaCopilotServer.Application.Arknights.GetLevelList;
using MaaCopilotServer.Application.Arknights.GetOperatorList;
using MaaCopilotServer.Application.Arknights.RemoveCustomLevels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller of copilot operations under <c>/arknights</c> endpoint,
///     including operations related to copilot operations.
/// </summary>
/// <remarks>
/// Response codes:
/// <list type="bullet">
///     <item>
///         <term>400</term>
///         <description>A bad request. Most cases are invalid request parameters.</description>
///     </item>
///     <item>
///         <term>401</term>
///         <description>An unauthorized request. You need to login and set Authorization header at first.</description>
///     </item>
///     <item>
///         <term>403</term>
///         <description>A forbidden request. You do not have permission to perform the operation.</description>
///     </item>
///     <item>
///         <term>404</term>
///         <description>Something is not found.</description>
///     </item>
///     <item>
///         <term>500</term>
///         <description>Some server errors happens.</description>
///     </item>
/// </list>
/// </remarks>
[ApiController]
[Route("arknights")]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status500InternalServerError)]
public class ArknightsController : MaaControllerBase
{
    /// <summary>
    ///     The constructor of <see cref="ArknightsController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public ArknightsController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     Gets arknights data version.
    /// </summary>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The version and sync status info.
    ///             Note that the status could be <c>ERROR</c> or <c>DISASTER</c>.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("version")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetDataVersionQueryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArkDataVersion()
    {
        return await GetResponse(new GetDataVersionQuery());
    }

    /// <summary>
    ///     Gets arknights level data.
    /// </summary>
    /// <param name="query">The query params.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The list of levels.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("level")]
    [ProducesResponseType(typeof(MaaApiResponseModel<List<GetLevelListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArkLevelList([FromQuery] GetLevelListQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     Gets arknights operator data.
    /// </summary>
    /// <param name="query">The query params.</param>
    /// <returns>An asynchronous operation representing the response.</returns>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The list of operators.
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [HttpGet("operator")]
    [ProducesResponseType(typeof(MaaApiResponseModel<List<GetOperatorListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArkOperatorList([FromQuery] GetOperatorListQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     Add or update custom level data.
    /// </summary>
    /// <param name="command">The level list.</param>
    /// <returns>An asynchronous operation representing the response.</returns>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             Total database changes count, added and updated level ids.
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [HttpPost("custom/add")]
    [ProducesResponseType(typeof(MaaApiResponseModel<AddOrUpdateCustomLevelsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> AddOrUpdateCustomLevels([FromBody] AddOrUpdateCustomLevelsCommandBatch command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Remove custom level data.
    /// </summary>
    /// <param name="command">The remove level ids.</param>
    /// <returns>An asynchronous operation representing the response.</returns>
    /// <remarks>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             Total database changes count and removed level ids.
    ///         </description>
    ///     </item>
    /// </list>
    /// </remarks>
    [HttpPost("custom/remove")]
    [ProducesResponseType(typeof(MaaApiResponseModel<RemoveCustomLevelsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RemoveCustomLevels([FromBody] RemoveCustomLevelsCommand command)
    {
        return await GetResponse(command);
    }
}
