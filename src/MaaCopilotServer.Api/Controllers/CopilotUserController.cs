// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Dtos;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LoginCopilotUserDto = MaaCopilotServer.Api.Dtos.LoginCopilotUserDto;

namespace MaaCopilotServer.Api.Controllers;

[ApiController]
[Route("user")]
public class CopilotUserController : MaaControllerBase
{
    public CopilotUserController(IMediator mediator) : base(mediator) { }

    [HttpPost("login")]
    public async Task<ActionResult> LoginCopilotUser([FromBody] LoginCopilotUserDto dto)
    {
        var request = new LoginCopilotUserCommand { Email = dto.Email, Password = dto.Password };
        return await GetResponse(request);
    }

    [HttpPost("create")]
    public async Task<ActionResult> CreateCopilotUser([FromBody] CreateCopilotUserDto dto)
    {
        var request = new CreateCopilotUserCommand { Email = dto.Email, Password = dto.Password, UserName = dto.UserName, Role = dto.Role };
        return await GetResponse(request);
    }
}
