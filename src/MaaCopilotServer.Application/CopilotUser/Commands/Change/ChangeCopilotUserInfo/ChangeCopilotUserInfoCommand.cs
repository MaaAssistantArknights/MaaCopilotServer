// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;

/// <summary>
///     The record of changing user info.
/// </summary>
[Authorized(UserRole.Admin)]
public record ChangeCopilotUserInfoCommand : IRequest<MaaApiResponse<GetCopilotUserDto>>
{
    /// <summary>
    ///     The user ID.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

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

    /// <summary>
    ///     The password.
    /// </summary>
    [JsonPropertyName("password")]
    [LogMasked]
    public string? Password { get; set; }

    /// <summary>
    ///     The role of the user.
    /// </summary>
    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public string? Role { get; set; }
}

/// <summary>
///     The handler of changing user info.
/// </summary>
public class
    ChangeCopilotUserInfoCommandHandler : IRequestHandler<ChangeCopilotUserInfoCommand, MaaApiResponse<GetCopilotUserDto>>
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
    ///     The constructor of <see cref="ChangeCopilotUserInfoCommandHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="secretService">The service for processing passwords and tokens.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public ChangeCopilotUserInfoCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ISecretService secretService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _secretService = secretService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    ///     Handles a request of changing user info.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with no contents if the request completes successfully.</returns>
    /// <exception cref="PipelineException">
    ///     Thrown when the user ID does not exist, or an internal error occurs, or the user permission is insufficient.
    /// </exception>
    public async Task<MaaApiResponse<GetCopilotUserDto>> Handle(ChangeCopilotUserInfoCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);

        if (user is null)
        {
            return MaaApiResponseHelper.NotFound<GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, request.UserId));
        }

        var @operator = await _dbContext.CopilotUsers.FirstOrDefaultAsync(
            x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);
        if (@operator is null)
        {
            return MaaApiResponseHelper.InternalError<GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException);
        }

        if (@operator.UserRole is UserRole.Admin && user.UserRole >= UserRole.Admin)
        {
            return MaaApiResponseHelper.Forbidden<GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                _apiErrorMessage.PermissionDenied);
        }

        if (request.Password is not null)
        {
            var hash = _secretService.HashPassword(request.Password);
            user.UpdatePassword(@operator.EntityId, hash);
        }

        if (string.IsNullOrEmpty(request.Email))
        {
            var exist = _dbContext.CopilotUsers.Any(x => x.Email == request.Email);
            if (exist)
            {
                return MaaApiResponseHelper.BadRequest<GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                    _apiErrorMessage.EmailAlreadyInUse);
            }
        }

        user.UpdateUserInfo(@operator.EntityId, request.Email, request.UserName, Enum.Parse<UserRole>(request.Role!));

        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok<GetCopilotUserDto>(null, _currentUserService.GetTrackingId());
    }
}
