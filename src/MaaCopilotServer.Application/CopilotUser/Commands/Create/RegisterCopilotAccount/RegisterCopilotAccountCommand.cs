// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.CopilotUser.Commands.RegisterCopilotAccount;

/// <summary>
///     The DTO for the RegisterCopilotAccount command.
/// </summary>
public record RegisterCopilotAccountCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user email.
    /// </summary>
    [Required]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     The username.
    /// </summary>
    [Required]
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    /// <summary>
    ///     The password.
    /// </summary>
    [JsonPropertyName("password")]
    [LogMasked]
    [Required]
    public string? Password { get; set; }
}

public class RegisterCopilotAccountCommandHandler : IRequestHandler<RegisterCopilotAccountCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly IMailService _mailService;
    private readonly ISecretService _secretService;
    private readonly IOptions<TokenOption> _tokenOption;

    public RegisterCopilotAccountCommandHandler(
        IOptions<TokenOption> tokenOption,
        ICurrentUserService currentUserService,
        IMaaCopilotDbContext dbContext,
        ISecretService secretService,
        IMailService mailService,
        ApiErrorMessage apiErrorMessage)
    {
        _tokenOption = tokenOption;
        _currentUserService = currentUserService;
        _dbContext = dbContext;
        _secretService = secretService;
        _mailService = mailService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(RegisterCopilotAccountCommand request,
        CancellationToken cancellationToken)
    {
        var emailExist = await _dbContext.CopilotUsers.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailExist)
        {
            return MaaApiResponseHelper.BadRequest(
                _apiErrorMessage.EmailAlreadyInUse);
        }

        var user = new Domain.Entities.CopilotUser(request.Email!, _secretService.HashPassword(request.Password!),
            request.UserName!, UserRole.User, null);
        _dbContext.CopilotUsers.Add(user);

        var (token, time) = _secretService.GenerateToken(user.EntityId,
            TimeSpan.FromMinutes(_tokenOption.Value.AccountActivationToken.ExpireTime));
        var result = await _mailService.SendEmailAsync(
            new EmailUserActivation(user.UserName, token, time.ToUtc8String(),
                _tokenOption.Value.AccountActivationToken.HasCallback),
            user.Email);

        if (result is false)
        {
            return MaaApiResponseHelper.InternalError(
                _apiErrorMessage.EmailSendFailed);
        }

        var tokenEntity = new CopilotToken(user.EntityId, TokenType.UserActivation, token, time);
        _dbContext.CopilotTokens.Add(tokenEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok();
    }
}
