// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

/// <summary>
///     The record of updating user password.
/// </summary>
[Authorized(UserRole.User, true)]
public record UpdateCopilotUserPasswordCommand : IRequest<MaaApiResponse<EmptyObject>>
{
    /// <summary>
    ///     The original password.
    /// </summary>
    [JsonPropertyName("original_password")]
    [NotLogged]
    public string? OriginalPassword { get; set; }

    /// <summary>
    ///     The new password.
    /// </summary>
    [JsonPropertyName("new_password")]
    [NotLogged]
    public string? NewPassword { get; set; }
}

/// <summary>
///     The handler of updating user password.
/// </summary>
public class UpdateCopilotUserPasswordCommandHandler : IRequestHandler<UpdateCopilotUserPasswordCommand,
    MaaApiResponse<EmptyObject>>
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
    ///     The constructor of <see cref="UpdateCopilotUserPasswordCommandHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="secretService">The service for processing passwords and tokens.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public UpdateCopilotUserPasswordCommandHandler(
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
    ///     Handles the request of changing user password.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with no contents if the request completes successfully.</returns>
    /// <exception cref="PipelineException">Thrown when an internal error occurs, or the original password is incorrect.</exception>
    public async Task<MaaApiResponse<EmptyObject>> Handle(UpdateCopilotUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == _currentUserService.GetUserIdentity()!.Value, cancellationToken);

        if (user is null)
        {
            throw new PipelineException(MaaApiResponseHelper.InternalError(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException));
        }

        var ok = _secretService.VerifyPassword(user!.Password, request.OriginalPassword!);

        if (ok is false)
        {
            throw new PipelineException(MaaApiResponseHelper.BadRequest(_currentUserService.GetTrackingId(),
                _apiErrorMessage.PasswordInvalid));
        }

        var hash = _secretService.HashPassword(request.NewPassword!);
        user.UpdatePassword(user.EntityId, hash);
        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok<EmptyObject>(null, _currentUserService.GetTrackingId());
    }
}
