// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using MaaCopilotServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.RefreshUserAccessToken;

/// <summary>
///     The DTO for refreshing the access token of a copilot user command.
/// </summary>
public record RefreshUserAccessTokenCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     Expired access token.
    /// </summary>
    [Required]
    [JsonPropertyName("access_token")]
    public string? ExpiredAccessToken { get; init; }
    
    /// <summary>
    ///     Refresh token.
    /// </summary>
    [Required]
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }
}

public class RefreshUserAccessTokenCommandHandler : IRequestHandler<RefreshUserAccessTokenCommand, MaaApiResponse>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;
    private readonly ApiErrorMessage _apiErrorMessage;

    public RefreshUserAccessTokenCommandHandler(
        IMaaCopilotDbContext dbContext,
        ISecretService secretService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _secretService = secretService;
        _apiErrorMessage = apiErrorMessage;
    }
    
    public async Task<MaaApiResponse> Handle(RefreshUserAccessTokenCommand request, CancellationToken cancellationToken)
    {
        // Get user id from access token
        var userId = _secretService.GetUserIdFromAccessToken(request.ExpiredAccessToken!);

        if (userId is null)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.AccessTokenInvalid);
        }
        
        // Find refresh token in database
        var currentRefreshToken = await _dbContext
            .CopilotUserRefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken);

        if (currentRefreshToken is null)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.RefreshTokenInvalid);
        }
        
        // Check if refresh token is valid
        if (currentRefreshToken.Expires < DateTimeOffset.UtcNow)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.RefreshTokenInvalid);
        }
        
        // Check if user id in refresh token matches user id from access token
        if (currentRefreshToken.UserId != userId)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.RefreshTokenInvalid);
        }
        
        // Get User
        var user = await _dbContext
            .CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);

        if (user is null)
        {
            return MaaApiResponseHelper.BadRequest(
                string.Format(_apiErrorMessage.UserWithIdNotFound!, userId));
        }
        
        // Remove current refresh token
        _dbContext.CopilotUserRefreshTokens.Remove(currentRefreshToken);
        
        // Calculate the upload count of the user
        var uploadCount = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Where(x => x.Author.EntityId == user.EntityId)
            .CountAsync(cancellationToken);
        
        // Generate JWT token and refresh token
        var (token, expire) = _secretService.GenerateJwtToken(user.EntityId);
        var (refreshToken, refreshExpire) = _secretService.GenerateRefreshToken();

        // Add refresh token to the database
        var entity = new CopilotUserRefreshToken(user.EntityId, refreshToken, refreshExpire);
        await _dbContext.CopilotUserRefreshTokens.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Build DTO
        var dto = new LoginCopilotUserDto(token, expire.ToIsoString(), refreshToken, refreshExpire.ToIsoString(),
            new GetCopilotUserDto(user.EntityId, user.UserName, user.UserRole, uploadCount, user.UserActivated));
        return MaaApiResponseHelper.Ok(dto);
    }
}
