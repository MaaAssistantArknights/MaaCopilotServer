// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Dtos;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LoginCopilotUserDto = MaaCopilotServer.Api.Dtos.LoginCopilotUserDto;

namespace MaaCopilotServer.Api.Controllers;

[ApiController]
[Route("user")]
public class CopilotUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public CopilotUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginCopilotUser([FromBody] LoginCopilotUserDto dto)
    {
        var request = new LoginCopilotUserCommand { Email = dto.Email, Password = dto.Password };
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

    [HttpPost("create")]
    public async Task<ActionResult> CreateCopilotUser([FromBody] CreateCopilotUserDto dto)
    {
        var request = new CreateCopilotUserCommand { Email = dto.Email, Password = dto.Password, UserName = dto.UserName, Role = dto.Role };
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
