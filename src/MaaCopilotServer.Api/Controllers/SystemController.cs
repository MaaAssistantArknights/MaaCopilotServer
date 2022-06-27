// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application.System.GetCurrentVersion;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller to get system infos.
/// </summary>
/// <response code="500">Some server errors happens.</response>
[ApiController]
[Route("")]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status500InternalServerError)]
public class SystemController : MaaControllerBase
{
    /// <summary>
    ///     The constructor of <see cref="SystemController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public SystemController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     Get the current version of the server.
    /// </summary>
    /// <response code="200">The current version of the server.</response>
    [HttpGet("version")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCurrentVersionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetVersion()
    {
        return await GetResponse(new GetCurrentVersionCommand());
    }
}
