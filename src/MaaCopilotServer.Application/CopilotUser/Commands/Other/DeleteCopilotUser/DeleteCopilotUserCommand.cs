// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

/// <summary>
///     The record of deleting user.
/// </summary>
[Authorized(UserRole.Admin)]
public record DeleteCopilotUserCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user ID.
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}

/// <summary>
///     The handler of deleting user.
/// </summary>
public class DeleteCopilotUserCommandHandler : IRequestHandler<DeleteCopilotUserCommand, MaaApiResponse>
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
    ///     The constructor of <see cref="DeleteCopilotUserCommandHandler" />.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public DeleteCopilotUserCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    ///     Handles a request of deleting user.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>
    ///     <para>A task with no contents if the request completes successfully.</para>
    ///     <para>403 when the user permission is insufficient.</para>
    ///     <para>404 when the user ID does not exist.</para>
    ///     <para>500 when an internal error occurs.</para>
    /// </returns>
    public async Task<MaaApiResponse> Handle(DeleteCopilotUserCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);
        if (user == null)
        {
            return MaaApiResponseHelper.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, userId));
        }

        var @operator = await _dbContext.CopilotUsers.FirstOrDefaultAsync(
            x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);
        if (@operator is null)
        {
            return MaaApiResponseHelper.InternalError(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException);
        }

        if (@operator.UserRole <= user.UserRole)
        {
            return MaaApiResponseHelper.Forbidden(_currentUserService.GetTrackingId(),
                _apiErrorMessage.PermissionDenied);
        }

        user.Delete(_currentUserService.GetUserIdentity()!.Value);
        _dbContext.CopilotUsers.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok(null, _currentUserService.GetTrackingId());
    }
}
