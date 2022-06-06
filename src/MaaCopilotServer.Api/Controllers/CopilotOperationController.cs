// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Dtos;
using MaaCopilotServer.Application.Common.Exceptions;
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
public class CopilotOperationController : ControllerBase
{
    private readonly IMediator _mediator;

    public CopilotOperationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("upload")]
    public async Task<ActionResult> CreateCopilotOperation([FromBody] CreateCopilotOperationDto dto)
    {
        var request = new CreateCopilotOperationCommand { Content = dto.Content };
        try
        {
            var response = await _mediator.Send(request);
            return response;
        }
        catch (PipelineException ex)
        {
            return ex.Result;
        }
    }

    [HttpPost("delete")]
    public async Task<ActionResult> DeleteCopilotOperation([FromBody] DeleteCopilotOperationDto dto)
    {
        var request = new DeleteCopilotOperationCommand { Id = dto.Id };
        try
        {
            var response = await _mediator.Send(request);
            return response;
        }
        catch (PipelineException ex)
        {
            return ex.Result;
        }
    }

    [HttpGet("get/{id}")]
    public async Task<ActionResult> GetCopilotOperation(string id)
    {
        var request = new GetCopilotOperationQuery { Id = id };
        try
        {
            var response = await _mediator.Send(request);
            return response;
        }
        catch (PipelineException ex)
        {
            return ex.Result;
        }
    }

    [HttpGet("get")]
    public async Task<ActionResult> QueryCopilotOperation(
        [FromQuery] int page = 1,
        [FromQuery] int limit = 10,
        [FromQuery(Name = "stage_name")] string stageName = "",
        [FromQuery] string content = "")
    {
        var request = new QueryCopilotOperationsQuery { Page = page, Limit = limit, StageName = stageName, Content = content };
        try
        {
            var response = await _mediator.Send(request);
            return response;
        }
        catch (PipelineException ex)
        {
            return ex.Result;
        }
    }
}
