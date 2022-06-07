// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Dtos;
using MaaCopilotServer.Application.CopilotOperation.Commands.CreateCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Commands.DeleteCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.GetCopilotOperation;
using MaaCopilotServer.Application.CopilotOperation.Queries.QueryCopilotOperations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CreateCopilotOperationDto = MaaCopilotServer.Api.Dtos.CreateCopilotOperationDto;

namespace MaaCopilotServer.Api.Controllers;

[ApiController]
[Route("copilot")]
public class CopilotOperationController : MaaControllerBase
{
    public CopilotOperationController(IMediator mediator) : base(mediator) { }

    [HttpPost("upload")]
    public async Task<ActionResult> CreateCopilotOperation([FromBody] CreateCopilotOperationDto dto)
    {
        var request = new CreateCopilotOperationCommand { Content = dto.Content };
        return await GetResponse(request);
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteCopilotOperation([FromBody] DeleteCopilotOperationDto dto)
    {
        var request = new DeleteCopilotOperationCommand { Id = dto.Id };
        return await GetResponse(request);
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult> GetCopilotOperation(string id)
    {
        var request = new GetCopilotOperationQuery { Id = id };
        return await GetResponse(request);
    }

    [HttpGet("get")]
    public async Task<ActionResult> QueryCopilotOperation(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery(Name = "stage_name")] string stageName = "",
        [FromQuery] string content = "")
    {
        var request = new QueryCopilotOperationsQuery { Page = page, Limit = limit, StageName = stageName, Content = content };
        return await GetResponse(request);
    }
}
