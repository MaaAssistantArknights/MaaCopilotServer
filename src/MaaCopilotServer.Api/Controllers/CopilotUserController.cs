// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Api.Swagger;
using MaaCopilotServer.Application.Common.Models;
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
using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller of copilot operations under "user" endpoint.
/// Include operations related to copilot users.
/// </summary>
/// <response code="400">A bad request, most cases are invalid request parameters.</response>
/// <response code="401">An unauthorized request, you need to login and set Authorization header at first.</response>
/// <response code="403">A forbidden request, you do not have permission to perform the operation.</response>
/// <response code="404">Some thing not found.</response>
/// <response code="500">Some server errors happens.</response>
[ApiController]
[Route("user")]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status500InternalServerError)]
public class CopilotUserController : MaaControllerBase
{
    /// <summary>
    ///     The constructor of <see cref="CopilotOperationController" />.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public CopilotUserController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     Change the user info.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The changes has benn applied successfully.</response>
    [HttpPost("change")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeCopilotUserInfo([FromBody] ChangeCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Create a new user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The copilot user has been created successfully.</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateCopilotUser([FromBody] CreateCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Delete a user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The copilot user has been deleted successfully.</response>
    [HttpPost("delete")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCopilotUser([FromBody] DeleteCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Login a user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">Login successfully.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(MaaApiResponseModel<LoginCopilotUserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> LoginCopilotUser([FromBody] LoginCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Update the user info, password not included.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">
    ///     The changes has benn applied successfully.
    ///     If the email has been changed, a new activation code will be sent to the new email.
    /// </response>
    [HttpPost("update/info")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateCopilotUserInfo([FromBody] UpdateCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Update the user password.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The changes has benn applied successfully.</response>
    [HttpPost("update/password")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateCopilotUserPassword([FromBody] UpdateCopilotUserPasswordCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Get the user info by id.
    /// </summary>
    /// <param name="id">The user id, or placeholders like me.</param>
    /// <response code="200">The user info in detail.</response>
    [HttpGet("info/{id}")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCopilotUserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCopilotUser(string? id)
    {
        var command = new GetCopilotUserQuery { UserId = id };
        return await GetResponse(command);
    }

    /// <summary>
    ///     Query users.
    /// </summary>
    /// <param name="query">The request body.</param>
    /// <response code="200">The brief user info list.</response>
    [HttpGet("query")]
    [ProducesResponseType(typeof(MaaApiResponseModel<PaginationResult<QueryCopilotUserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> QueryCopilotUser([FromQuery] QueryCopilotUserQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     Register a user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The copilot user has been created successfully and an activation code has been sent to the email address.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RegisterAccount([FromBody] RegisterCopilotAccountCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Activate a user account.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The account has been activated successfully.</response>
    [HttpPost("activate")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ActivateAccount([FromBody] ActivateCopilotAccountCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Request a password reset.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The password reset token has been sent to the email address.</response>
    [HttpPost("password/reset_request")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RequestPasswordChange([FromBody] RequestPasswordResetCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Reset password.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">The password has been reset successfully.</response>
    [HttpPost("password/reset")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> PasswordChange([FromBody] PasswordResetCommand command)
    {
        return await GetResponse(command);
    }
}
