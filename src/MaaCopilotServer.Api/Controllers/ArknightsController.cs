// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application.Arknights.GetDataVersion;
using MaaCopilotServer.Application.Arknights.GetLevelList;
using MaaCopilotServer.Application.Arknights.GetOperatorList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller of copilot operations under "copilot" endpoint.
/// Include operations related to copilot operations.
/// </summary>
/// <response code="400">A bad request, most cases are invalid request parameters.</response>
/// <response code="401">An unauthorized request, you need to login and set Authorization header at first.</response>
/// <response code="403">A forbidden request, you do not have permission to perform the operation.</response>
/// <response code="404">Some thing not found.</response>
/// <response code="500">Some server errors happens.</response>
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
    ///     Get arknights data version.
    /// </summary>
    /// <response code="200">The version info.</response>
    [HttpGet("version")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetDataVersionQueryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArkDataVersion()
    {
        return await GetResponse(new GetDataVersionQuery());
    }

    /// <summary>
    ///     Update a copilot operation.
    /// </summary>
    /// <param name="query">The query params.</param>
    /// <response code="200">The list of levels</response>
    [HttpGet("level")]
    [ProducesResponseType(typeof(MaaApiResponseModel<List<GetLevelListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArkLevelList([FromQuery] GetLevelListQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     Update a copilot operation.
    /// </summary>
    /// <param name="query">The query params.</param>
    /// <response code="200">The list of operators</response>
    [HttpGet("operator")]
    [ProducesResponseType(typeof(MaaApiResponseModel<List<GetOperatorListDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetArkOperatorList([FromQuery] GetOperatorListQuery query)
    {
        return await GetResponse(query);
    }
}
