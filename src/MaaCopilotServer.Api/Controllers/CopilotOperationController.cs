// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

[ApiController]
[Route("copilot")]
public class CopilotOperationController : MaaControllerBase
{
    public CopilotOperationController(IMediator mediator) : base(mediator) { }

    [HttpPost("upload")]
    public async Task<ActionResult> CreateCopilotOperation([FromBody] CreateCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteCopilotOperation([FromBody] DeleteCopilotOperationCommand command)
    {
        return await GetResponse(command);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult> GetCopilotOperation(string id)
    {
        var request = new GetCopilotOperationQuery { Id = id };
        return await GetResponse(request);
    }

    [HttpGet("query")]
    public async Task<ActionResult> QueryCopilotOperation([FromQuery] QueryCopilotOperationsQuery query)
    {
        return await GetResponse(query);
    }
}
