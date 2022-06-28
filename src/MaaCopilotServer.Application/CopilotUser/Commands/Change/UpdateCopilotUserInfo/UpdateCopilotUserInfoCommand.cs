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
///     The DTO for the UpdateCopilotUserInfo command.
/// </summary>
[Authorized(UserRole.User, true)]
public record UpdateCopilotUserInfoCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user email. Set this value to update the email.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     The username. Set this value to update the username.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }
}

public class UpdateCopilotUserInfoCommandHandler : IRequestHandler<UpdateCopilotUserInfoCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly IMailService _mailService;
    private readonly ISecretService _secretService;
    private readonly IOptions<TokenOption> _tokenOption;

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

    public async Task<MaaApiResponse> Handle(UpdateCopilotUserInfoCommand request, CancellationToken cancellationToken)
    {
        // Get current infos.
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Id email change is requested
        if (string.IsNullOrEmpty(request.Email) is false)
        {
            // Check if the new email has already been used by someone
            var exist = _dbContext.CopilotUsers.Any(x => x.Email == request.Email);
            if (exist)
            {
                return MaaApiResponseHelper.BadRequest(_apiErrorMessage.EmailAlreadyInUse);
            }

            // Generate a activation token
            var (token, time) = _secretService.GenerateToken(user.EntityId, TimeSpan.FromMinutes(
                _tokenOption.Value.ChangeEmailToken.ExpireTime));
            // Send email
            var result = await _mailService.SendEmailAsync(
                new EmailChangeAddress(user.UserName, token, time.ToUtc8String(),
                    _tokenOption.Value.AccountActivationToken.HasCallback),
                request.Email);

            // If email failed to send, return an internal error response
            if (result is false)
            {
                return MaaApiResponseHelper.InternalError(_apiErrorMessage.EmailSendFailed);
            }
        }

        // Update user info, default to Role = null, Force = false
        user.UpdateUserInfo(user.EntityId, request.Email, request.UserName);
        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
