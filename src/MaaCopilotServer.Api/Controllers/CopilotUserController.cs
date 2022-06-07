// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

[ApiController]
[Route("user")]
public class CopilotUserController : MaaControllerBase
{
    public CopilotUserController(IMediator mediator) : base(mediator) { }

    [HttpPost("change")]
    public async Task<ActionResult> ChangeCopilotUserInfo([FromBody] ChangeCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateCopilotUser([FromBody] CreateCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginCopilotUser([FromBody] LoginCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("update/info")]
    public async Task<ActionResult> UpdateCopilotUserInfo([FromBody] UpdateCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    [HttpPost("update/password")]
    public async Task<ActionResult> UpdateCopilotUserPassword([FromBody] UpdateCopilotUserPasswordCommand command)
    {
        return await GetResponse(command);
    }

    [HttpGet("info/{id}")]
    public async Task<ActionResult> GetCopilotUser(string? id)
    {
        var command = new GetCopilotUserQuery { UserId = id };
        return await GetResponse(command);
    }

    [HttpGet("query")]
    public async Task<ActionResult> QueryCopilotUser([FromQuery] QueryCopilotUserQuery query)
    {
        return await GetResponse(query);
    }
}
