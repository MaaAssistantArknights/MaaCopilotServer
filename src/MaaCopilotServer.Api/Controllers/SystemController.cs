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
///     The controller to get system infos under <c>/</c> endpoint.
/// </summary>
/// <remarks>
/// Response codes:
/// <list type="bullet">
///     <item>
///         <term>500</term>
///         <description>Some server errors happens.</description>
///     </item>
/// </list>
/// </remarks>
[ApiController]
[Route("")]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status500InternalServerError)]
public class SystemController : MaaControllerBase
{
    /// <summary>
    /// The copilot server options.
    /// </summary>
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
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The server is ready.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("")]
    public async Task<ActionResult> Echo()
    {
        return await GetResponse(new EchoCommand());
    }

    /// <summary>
    ///     Gets the current version of the server.
    /// </summary>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The current version of the server.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("version")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCurrentVersionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetVersion()
    {
        return await GetResponse(new GetCurrentVersionCommand());
    }

    /// <summary>
    ///     Sends a test email.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The email has been sent successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>HTTP 404</term>
    ///         <description>
    ///             The test email feature is not switched on.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// This API only available when <c>EnableTestEmailApi</c> settings in <c>appsettings.json</c> is <c>true</c>.
    /// Otherwise, this API will return a 404 error.
    /// </remarks>
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
