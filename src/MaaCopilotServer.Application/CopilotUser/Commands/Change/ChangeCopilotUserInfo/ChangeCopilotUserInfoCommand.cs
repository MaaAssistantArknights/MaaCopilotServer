// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;

/// <summary>
///     The DTO for the change copilot user info command.
/// </summary>
[Authorized(UserRole.Admin)]
public record ChangeCopilotUserInfoCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user id.
    /// </summary>
    [Required]
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    /// <summary>
    ///     The user email. Set this value to change the email.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     The username. Set this value to change the username.
    /// </summary>
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    /// <summary>
    ///     The password. Set this value to change the password.
    /// </summary>
    [JsonPropertyName("password")]
    [LogMasked]
    public string? Password { get; set; }

    /// <summary>
    ///     The role of the user. Set this value to change the role.
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}

public class ChangeCopilotUserInfoCommandHandler : IRequestHandler<ChangeCopilotUserInfoCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;

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

    public async Task<MaaApiResponse> Handle(ChangeCopilotUserInfoCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);

        if (user is null)
        {
            return MaaApiResponseHelper.NotFound(string.Format(_apiErrorMessage.UserWithIdNotFound!, request.UserId));
        }

        var @operator = await _dbContext.CopilotUsers.FirstOrDefaultAsync(
            x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);
        if (@operator is null)
        {
            return MaaApiResponseHelper.InternalError(_apiErrorMessage.InternalException);
        }

        if (@operator.UserRole is UserRole.Admin && user.UserRole >= UserRole.Admin)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied);
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
                return MaaApiResponseHelper.BadRequest(_apiErrorMessage.EmailAlreadyInUse);
            }
        }

        user.UpdateUserInfo(@operator.EntityId, request.Email, request.UserName, Enum.Parse<UserRole>(request.Role!));

        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok();
    }
}
