// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application.System.Echo;
using MaaCopilotServer.Application.System.GetCurrentVersion;
using MaaCopilotServer.Application.System.SendEmailTest;
using MaaCopilotServer.Domain.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
    private readonly IOptions<CopilotServerOption> _options;

    /// <summary>
    ///     The constructor of <see cref="SystemController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    /// <param name="options">The copilot server options.</param>
    public SystemController(IMediator mediator, IOptions<CopilotServerOption> options) : base(mediator)
    {
        _options = options;
    }

    /// <summary>
    /// Tests if the server is ready.
    /// </summary>
    /// <returns>The task indicating the result.</returns>
    [HttpGet("")]
    public async Task<ActionResult> Echo()
    {
        return await GetResponse(new EchoCommand());
    }

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

    /// <summary>
    ///     Send a test email.
    /// </summary>
    /// <remarks>
    ///     This API only available when EnableTestEmailApi settings in appsettings.json is true. Otherwise, this API will return a 404 error.
    /// </remarks>
    /// <param name="command">The request body.</param>
    /// <response code="200">The email was sent successfully.</response>
    [HttpGet("test/email")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> SendEmailTest([FromQuery] SendEmailTestCommand command)
    {
        if (_options.Value.EnableTestEmailApi is false)
        {
            return new NotFoundResult();
        }
        return await GetResponse(command);
    }
}
