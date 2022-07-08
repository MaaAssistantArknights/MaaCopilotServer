// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Email.Models;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using MaaCopilotServer.Domain.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.CopilotUser.Commands.RequestActivationToken;

/// <summary>
///     The DTO for the request activation token command. The request body is empty.
/// </summary>
[Authorized(UserRole.User, true)]
public record RequestActivationTokenCommand : IRequest<MaaApiResponse>;

public class RequestActivationTokenCommandHandler : IRequestHandler<RequestActivationTokenCommand, MaaApiResponse>
{
    private readonly IOptions<TokenOption> _tokenOption;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly IMailService _mailService;
    private readonly ISecretService _secretService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;

    public RequestActivationTokenCommandHandler(
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

    public async Task<MaaApiResponse> Handle(RequestActivationTokenCommand request, CancellationToken cancellationToken)
    {
        // Get current infos.
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Check if the user is already activated.
        if (user.UserActivated)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.UserAlreadyActivated);
        }

        // Query token from the database.
        var token = await _dbContext.CopilotTokens
            .Where(x => x.Type == TokenType.UserActivation)
            .Where(x => x.ResourceId == user.EntityId)
            .Where(x => x.ValidBefore > DateTimeOffset.UtcNow)
            .FirstOrDefaultAsync(cancellationToken);

        // If token exist and not expired, forbidden.
        if (token is not null)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.TokenAlreadyExist);
        }

        // If token not exist or expired, create a new one.
        var (newToken, newExpireTime) = _secretService
            .GenerateToken(
                user.EntityId,
                TimeSpan.FromMinutes(
                    _tokenOption.Value.AccountActivationToken.ExpireTime)
                );

        // Send email
        var result = await _mailService.SendEmailAsync(
            new EmailUserActivation(
                user.UserName, newToken,
                newExpireTime.ToUtc8String(),
                _tokenOption.Value.AccountActivationToken.HasCallback),
            user.Email);
        if (result is false)
        {
            return MaaApiResponseHelper.InternalError(_apiErrorMessage.EmailSendFailed);
        }

        // Add token to database
        var newTokenEntity = new CopilotToken(user.EntityId, TokenType.UserActivation, newToken, newExpireTime);
        _dbContext.CopilotTokens.Add(newTokenEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok();
    }
}
