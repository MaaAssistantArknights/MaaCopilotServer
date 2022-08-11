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
using MaaCopilotServer.Application.CopilotUser.Commands.RequestActivationToken;
using MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;
using MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Queries.QueryCopilotUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MaaCopilotServer.Api.Controllers;

/// <summary>
///     The controller of copilot operations under <c>/user</c> endpoint,
///     including operations related to copilot users.
/// </summary>
/// <remarks>
/// Response codes:
/// <list type="bullet">
///     <item>
///         <term>400</term>
///         <description>A bad request. Most cases are invalid request parameters.</description>
///     </item>
///     <item>
///         <term>401</term>
///         <description>An unauthorized request. You need to login and set Authorization header at first.</description>
///     </item>
///     <item>
///         <term>403</term>
///         <description>A forbidden request. You do not have permission to perform the operation.</description>
///     </item>
///     <item>
///         <term>404</term>
///         <description>Something is not found.</description>
///     </item>
///     <item>
///         <term>500</term>
///         <description>Some server errors happens.</description>
///     </item>
/// </list>
/// </remarks>
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
    ///     Changes the user info.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The changes have been applied successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The email is already in use.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>403</term>
    ///         <description>
    ///             You are not authorised to edit the user info. This happens when:
    ///             <list type="bullet">
    ///                 <item>The operator is an Admin, and the target has the role above or equal to Admin; or</item>
    ///                 <item>The operator is a Super Admin, and there are role changes.</item>
    ///             </list>
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>
    ///             The user specified is not found.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("change")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeCopilotUserInfo([FromBody] ChangeCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Creates a new user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The copilot user has been created successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The email is already in use.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateCopilotUser([FromBody] CreateCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Deletes a user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The copilot user has been deleted successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>403</term>
    ///         <description>
    ///             You are not authorised to delete the user.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>
    ///             The user specified is not found.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("delete")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCopilotUser([FromBody] DeleteCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     User login.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The user has logged in successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             Either the username or the password is wrong, or both are wrong.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(MaaApiResponseModel<LoginCopilotUserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> LoginCopilotUser([FromBody] LoginCopilotUserCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Updates the user info, except the password.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <response code="200">
    ///     The changes has benn applied successfully.
    ///     If the email has been changed, a new activation code will be sent to the new email.
    /// </response>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The changes has benn applied successfully.
    ///             If the email has been changed, a new activation code will be sent to the new email.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The email is already in use.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>500</term>
    ///         <description>
    ///             An error occurred when sending the email.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("update/info")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateCopilotUserInfo([FromBody] UpdateCopilotUserInfoCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Updates the user password.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The changes have been applied successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The original password is incorrect.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("update/password")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateCopilotUserPassword([FromBody] UpdateCopilotUserPasswordCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Gets the user info by ID.
    /// </summary>
    /// <param name="id">The user ID, or placeholders like <c>me</c>.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The user info in detail.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The user ID is set to <c>me</c>, but the requester has not logged in.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>404</term>
    ///         <description>
    ///             The user specified is not found.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("info/{id}")]
    [ProducesResponseType(typeof(MaaApiResponseModel<GetCopilotUserDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetCopilotUser(string? id)
    {
        var command = new GetCopilotUserQuery { UserId = id };
        return await GetResponse(command);
    }

    /// <summary>
    ///     Queries users.
    /// </summary>
    /// <param name="query">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The brief user info list.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpGet("query")]
    [ProducesResponseType(typeof(MaaApiResponseModel<PaginationResult<QueryCopilotUserDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult> QueryCopilotUser([FromQuery] QueryCopilotUserQuery query)
    {
        return await GetResponse(query);
    }

    /// <summary>
    ///     Registers a user.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The copilot user has been created successfully
    ///             and an activation code has been sent to the email address.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The email is already in use.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>500</term>
    ///         <description>
    ///             An error occurred when sending the email.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RegisterAccount([FromBody] RegisterCopilotAccountCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Activates a user account.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The account has been activated successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The token is incorrect, or has expired.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>500</term>
    ///         <description>
    ///             The user to whom the token belongs is missing.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("activate")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> ActivateAccount([FromBody] ActivateCopilotAccountCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Requests a password reset.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The password reset token has been sent to the email address.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The email has not been registered.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>500</term>
    ///         <description>
    ///             An error occurred when sending the email.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("password/reset_request")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RequestPasswordChange([FromBody] RequestPasswordResetCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Resets password.
    /// </summary>
    /// <param name="command">The request body.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The password has been reset successfully.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The token is incorrect, or has expired.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>500</term>
    ///         <description>
    ///             The user to whom the token belongs is missing.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("password/reset")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> PasswordChange([FromBody] PasswordResetCommand command)
    {
        return await GetResponse(command);
    }

    /// <summary>
    ///     Requests a new activation code.
    /// </summary>
    /// <param name="command">The request body, which could be empty.</param>
    /// <returns>
    /// <list type="bullet">
    ///     <item>
    ///         <term>200</term>
    ///         <description>
    ///             The activation code has been sent to the email address.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>400</term>
    ///         <description>
    ///             The user has been activated, or there is already an activation token for the user.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>500</term>
    ///         <description>
    ///             The user to whom the token belongs is missing.
    ///         </description>
    ///     </item>
    /// </list>
    /// </returns>
    [HttpPost("activate/request")]
    [ProducesResponseType(typeof(MaaApiResponseModel<EmptyObjectModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult> RequestActivationCode(RequestActivationTokenCommand? command = null)
    {
        return await GetResponse(command ?? new RequestActivationTokenCommand());
    }
}
