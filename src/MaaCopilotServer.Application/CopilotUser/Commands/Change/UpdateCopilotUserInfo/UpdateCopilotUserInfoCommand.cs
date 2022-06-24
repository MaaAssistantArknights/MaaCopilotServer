// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;

/// <summary>
///     The record of updating user info.
/// </summary>
[Authorized(UserRole.User, true)]
public record UpdateCopilotUserInfoCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user email.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     The username.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }
}

/// <summary>
///     The handler of updating user info.
/// </summary>
public class
    UpdateCopilotUserInfoCommandHandler : IRequestHandler<UpdateCopilotUserInfoCommand, MaaApiResponse>
{
    /// <summary>
    ///     The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage;

    /// <summary>
    ///     The service for current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext;

    private readonly IMailService _mailService;

    private readonly ISecretService _secretService;

    private readonly IOptions<TokenOption> _tokenOption;

    /// <summary>
    ///     The constructor of <see cref="UpdateCopilotUserInfoCommandHandler" />.
    /// </summary>
    /// <param name="tokenOption"></param>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="mailService">The mail service.</param>
    /// <param name="secretService">The secret service.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public UpdateCopilotUserInfoCommandHandler(
        IOptions<TokenOption> tokenOption,
        IMaaCopilotDbContext dbContext,
        IMailService mailService,
        ISecretService secretService,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _tokenOption = tokenOption;
        _dbContext = dbContext;
        _mailService = mailService;
        _secretService = secretService;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    ///     Handles a request of updating user info.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task with no contents if the request completes successfully.</para>
    ///     <para>400 when the email is already in use.</para>
    ///     <para>500 when an internal error occurs.</para>
    /// </returns>
    public async Task<MaaApiResponse> Handle(UpdateCopilotUserInfoCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);

        if (user is null)
        {
            return MaaApiResponseHelper.InternalError(_apiErrorMessage.InternalException);
        }

        if (string.IsNullOrEmpty(request.Email) is false)
        {
            var exist = _dbContext.CopilotUsers.Any(x => x.Email == request.Email);
            if (exist)
            {
                return MaaApiResponseHelper.BadRequest(_apiErrorMessage.EmailAlreadyInUse);
            }

            var (token, time) = _secretService.GenerateToken(user.EntityId, TimeSpan.FromMinutes(
                _tokenOption.Value.ChangeEmailToken.ExpireTime));
            var result = await _mailService.SendEmailAsync(
                new EmailChangeAddress(user.UserName, token, time.ToUtc8String(),
                    _tokenOption.Value.AccountActivationToken.HasCallback),
                request.Email);

            if (result is false)
            {
                return MaaApiResponseHelper.InternalError(_apiErrorMessage.EmailSendFailed);
            }
        }

        user.UpdateUserInfo(user.EntityId, request.Email, request.UserName);
        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
