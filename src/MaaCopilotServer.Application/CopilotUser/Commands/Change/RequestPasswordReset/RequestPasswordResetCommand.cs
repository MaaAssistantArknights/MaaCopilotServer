// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.CopilotUser.Commands.RequestPasswordReset;

/// <summary>
///     The DTO for the request password reset command.
/// </summary>
public record RequestPasswordResetCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The email address of the user.
    /// </summary>
    [Required]
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}

public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly IMailService _mailService;
    private readonly ISecretService _secretService;
    private readonly IOptions<TokenOption> _tokenOption;

    public RequestPasswordResetCommandHandler(
        IOptions<TokenOption> tokenOption,
        IMaaCopilotDbContext dbContext,
        ISecretService secretService,
        IMailService mailService,
        ApiErrorMessage apiErrorMessage)
    {
        _tokenOption = tokenOption;
        _dbContext = dbContext;
        _secretService = secretService;
        _mailService = mailService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(RequestPasswordResetCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (user is null)
        {
            return MaaApiResponseHelper.NotFound(_apiErrorMessage.EmailNotRegister);
        }

        var alreadyHaveToken = await _dbContext.CopilotTokens.FirstOrDefaultAsync(
            x => x.ResourceId == user.EntityId && x.Type == TokenType.UserPasswordReset, cancellationToken);
        if (alreadyHaveToken is not null)
        {
            alreadyHaveToken.Delete(user.EntityId);
            _dbContext.CopilotTokens.Remove(alreadyHaveToken);
        }

        var (token, time) = _secretService.GenerateToken(user.EntityId,
            TimeSpan.FromMinutes(_tokenOption.Value.PasswordResetToken.ExpireTime));
        var success = await _mailService.SendEmailAsync(
            new EmailPasswordReset(user.UserName, token, time.ToUtc8String(),
                _tokenOption.Value.PasswordResetToken.HasCallback), user.Email);

        if (success is false)
        {
            return MaaApiResponseHelper.InternalError(_apiErrorMessage.EmailSendFailed);
        }

        _dbContext.CopilotTokens.Add(new CopilotToken(user.EntityId, TokenType.UserPasswordReset, token, time));
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
