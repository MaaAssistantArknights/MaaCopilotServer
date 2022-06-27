// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.System.GetCurrentVersion;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

[ApiController]
[Route("")]
public class SystemController : MaaControllerBase
{
    public SystemController(IMediator mediator) : base(mediator) { }

    [HttpGet("version")]
    public async Task<ActionResult> GetVersion()
    {
        return await GetResponse(new GetCurrentVersionCommand());
    }
}
