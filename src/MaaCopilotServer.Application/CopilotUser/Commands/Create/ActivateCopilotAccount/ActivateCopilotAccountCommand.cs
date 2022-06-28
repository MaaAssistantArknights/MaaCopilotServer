// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;

/// <summary>
///     The DTO for the ActivateCopilotAccount command.
/// </summary>
public record ActivateCopilotAccountCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     Account activation token.
    /// </summary>
    [Required]
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}

public class ActivateCopilotAccountCommandHandler : IRequestHandler<ActivateCopilotAccountCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public ActivateCopilotAccountCommandHandler(
        ICurrentUserService currentUserService,
        IMaaCopilotDbContext dbContext,
        ApiErrorMessage apiErrorMessage)
    {
        _currentUserService = currentUserService;
        _dbContext = dbContext;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(ActivateCopilotAccountCommand request,
        CancellationToken cancellationToken)
    {
        // Get token entity
        var token = await _dbContext.CopilotTokens.FirstOrDefaultAsync(x => x.Token == request.Token,
            cancellationToken);

        // If token is null OR token is not valid now OR token is not for UserActivation, return a bad request response.
        if (token is null || token.ValidBefore < DateTimeOffset.UtcNow || token.Type != TokenType.UserActivation)
        {
            return MaaApiResponseHelper.BadRequest(
                _apiErrorMessage.TokenInvalid);
        }

        // Find user defined in token entity
        var user = _dbContext.CopilotUsers.FirstOrDefault(x => x.EntityId == token.ResourceId);
        if (user is null)
        {
            return MaaApiResponseHelper.InternalError(
                string.Format(_apiErrorMessage.UserWithIdNotFound!, token.ResourceId.ToString()));
        }

        // Activate user and delete the token
        user.ActivateUser(user.EntityId);
        token.Delete(user.EntityId);
        _dbContext.CopilotUsers.Update(user);
        _dbContext.CopilotTokens.Remove(token);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok();
    }
}
