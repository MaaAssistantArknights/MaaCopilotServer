// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;

/// <summary>
///     The DTO for the PasswordReset command.
/// </summary>
public record PasswordResetCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The token to validate this request.
    /// </summary>
    [Required]
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    /// <summary>
    ///     The new password.
    /// </summary>
    [JsonPropertyName("password")]
    [LogMasked]
    [Required]
    public string? Password { get; set; }
}

public class PasswordResetCommandHandler :
    IRequestHandler<PasswordResetCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;

    public PasswordResetCommandHandler(
        ISecretService secretService,
        IMaaCopilotDbContext dbContext,
        ApiErrorMessage apiErrorMessage)
    {
        _secretService = secretService;
        _dbContext = dbContext;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(PasswordResetCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _dbContext.CopilotTokens.FirstOrDefaultAsync(x => x.Token == request.Token,
            cancellationToken);
        if (token is null || token.ValidBefore < DateTimeOffset.UtcNow || token.Type != TokenType.UserPasswordReset)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.TokenInvalid);
        }

        var user = _dbContext.CopilotUsers.FirstOrDefault(x => x.EntityId == token.ResourceId);
        if (user is null)
        {
            return MaaApiResponseHelper.InternalError(string.Format(_apiErrorMessage.UserWithIdNotFound!, token.ResourceId.ToString()));
        }

        user.UpdatePassword(user.EntityId, _secretService.HashPassword(request.Password!));

        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
