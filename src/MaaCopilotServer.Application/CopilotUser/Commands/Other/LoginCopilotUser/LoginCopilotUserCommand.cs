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

/// <summary>
///     The handler of user login.
/// </summary>
public class
    LoginCopilotUserCommandHandler : IRequestHandler<LoginCopilotUserCommand, MaaApiResponse>
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

    /// <summary>
    ///     The service for processing passwords and tokens.
    /// </summary>
    private readonly ISecretService _secretService;

    /// <summary>
    ///     The constructor of <see cref="LoginCopilotUserCommandHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="secretService">The service for processing passwords and tokens.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public LoginCopilotUserCommandHandler(
        IMaaCopilotDbContext dbContext,
        ISecretService secretService,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _secretService = secretService;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    ///     Handles the request of user login.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task with username, user token and token expiration time.</para>
    ///     <para>400 when the email does not exist, or the password is incorrect.</para>
    /// </returns>
    public async Task<MaaApiResponse> Handle(LoginCopilotUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers
            .Include(x => x.UserFavorites)
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (user is null)
        {
            return MaaApiResponseHelper.BadRequest(
                _apiErrorMessage.LoginFailed);
        }

        var ok = _secretService.VerifyPassword(user.Password, request.Password!);
        if (ok is false)
        {
            return MaaApiResponseHelper.BadRequest(
                _apiErrorMessage.LoginFailed);
        }

        var uploadCount = await _dbContext.CopilotOperations
            .Include(x => x.Author)
            .Where(x => x.Author.EntityId == user.EntityId)
            .CountAsync(cancellationToken);

        var (token, expire) = _secretService.GenerateJwtToken(user.EntityId);
        var favList = user.UserFavorites
            .ToDictionary(fav => fav.EntityId.ToString(), fav => fav.FavoriteName);
        var dto = new LoginCopilotUserDto(token, expire.ToIsoString(),
            new GetCopilotUserDto(user.EntityId, user.UserName, user.UserRole, uploadCount, user.UserActivated,
                favList));
        return MaaApiResponseHelper.Ok(dto);
    }
}
