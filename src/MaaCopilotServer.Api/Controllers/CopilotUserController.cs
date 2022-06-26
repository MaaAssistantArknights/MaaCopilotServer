// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;
using MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;
using MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUserFavorites;
using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller of user operations under <c>user</c> endpoint.
/// </summary>
[ApiController]
[Route("user")]
public class CopilotUserController : MaaControllerBase
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperationController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public CopilotUserController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     The handler of <c>change</c> endpoint to change user info.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("change")]
    public async Task<ActionResult> ChangeCopilotUserInfo([FromBody] ChangeCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>create</c> endpoint to create a new copilot user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("create")]
    public async Task<ActionResult> CreateCopilotUser([FromBody] CreateCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>delete</c> endpoint to delete a copilot user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("delete")]
    public async Task<ActionResult> DeleteCopilotUser([FromBody] DeleteCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>login</c> endpoint to login.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("login")]
    public async Task<ActionResult> LoginCopilotUser([FromBody] LoginCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>update/info</c> endpoint to update copilot user info.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("update/info")]
    public async Task<ActionResult> UpdateCopilotUserInfo([FromBody] UpdateCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>update/password</c> endpoint to set new password.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("update/password")]
    public async Task<ActionResult> UpdateCopilotUserPassword([FromBody] UpdateCopilotUserPasswordCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>info/:id</c> endpoint to get copilot user info.
    /// </summary>
    /// <param name="id">The path parameter <c>id</c>, which is the ID of the user.</param>
    /// <returns>The response.</returns>
    [HttpGet("info/{id}")]
    public async Task<ActionResult> GetCopilotUser(string? id)
    {
        var command = new GetCopilotUserQuery { UserId = id };
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>query</c> endpoint to query copilot user info.
    /// </summary>
    /// <param name="query">The query data.</param>
    /// <returns>The response.</returns>
    [HttpGet("query")]
    public async Task<ActionResult> QueryCopilotUser([FromQuery] QueryCopilotUserQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     The handler of <c>register</c> endpoint to register a new copilot user account.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAccount([FromBody] RegisterCopilotAccountCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>activate</c> endpoint to activate a copilot user account.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("activate")]
    public async Task<ActionResult> ActivateAccount([FromBody] ActivateCopilotAccountCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>password/reset_request</c> endpoint to request password reset.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("password/reset_request")]
    public async Task<ActionResult> RequestPasswordChange([FromBody] RequestPasswordResetCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     The handler of <c>password/reset</c> endpoint to confirm password reset.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>The response.</returns>
    [HttpPost("password/reset")]
    public async Task<ActionResult> PasswordChange([FromBody] PasswordResetCommand command)
    {
        return await GetResponse(command);
    }
}
