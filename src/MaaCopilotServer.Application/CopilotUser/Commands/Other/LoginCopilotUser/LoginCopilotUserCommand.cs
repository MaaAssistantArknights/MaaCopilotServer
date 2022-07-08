// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Application.CopilotUser.Queries.GetCopilotUser;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

/// <summary>
///     The DTO for the login command.
/// </summary>
public record LoginCopilotUserCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user email.
    /// </summary>
    [Required]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     The password.
    /// </summary>
    [JsonPropertyName("password")]
    [LogMasked]
    [Required]
    public string? Password { get; set; }
}

public class LoginCopilotUserCommandHandler : IRequestHandler<LoginCopilotUserCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;

    public LoginCopilotUserCommandHandler(
        IMaaCopilotDbContext dbContext,
        ISecretService secretService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _secretService = secretService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(LoginCopilotUserCommand request, CancellationToken cancellationToken)
    {
        // Find the user by email address
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (user is null)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.LoginFailed);
        }

        // Verify user password
        var ok = _secretService.VerifyPassword(user.Password, request.Password!);
        if (ok is false)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.LoginFailed);
        }

        // Calculate the upload count of the user
        var uploadCount = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Where(x => x.Author.EntityId == user.EntityId)
            .CountAsync(cancellationToken);

        // Generate JWT token
        var (token, expire) = _secretService.GenerateJwtToken(user.EntityId);

        // Build DTO
        var dto = new LoginCopilotUserDto(token, expire.ToIsoString(),
            new GetCopilotUserDto(user.EntityId, user.UserName, user.UserRole, uploadCount, user.UserActivated));
        return MaaApiResponseHelper.Ok(dto);
    }
}
