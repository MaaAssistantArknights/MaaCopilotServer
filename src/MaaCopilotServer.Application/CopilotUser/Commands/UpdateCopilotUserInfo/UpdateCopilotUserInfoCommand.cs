// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;

/// <summary>
/// The record of updating user info.
/// </summary>
[Authorized(UserRole.User)]
public record UpdateCopilotUserInfoCommand : IRequest<MaaActionResult<EmptyObject>>
{
    /// <summary>
    /// The user email.
    /// </summary>
    [JsonPropertyName("email")] public string? Email { get; set; }

    /// <summary>
    /// The username.
    /// </summary>
    [JsonPropertyName("user_name")] public string? UserName { get; set; }
}

/// <summary>
/// The handler of updating user info.
/// </summary>
public class
    UpdateCopilotUserInfoCommandHandler : IRequestHandler<UpdateCopilotUserInfoCommand, MaaActionResult<EmptyObject>>
{
    /// <summary>
    /// The service for current user.
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// The API error message.
    /// </summary>
    private readonly ApiErrorMessage _apiErrorMessage;

    /// <summary>
    /// The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _dbContext;

    /// <summary>
    /// The constructor of <see cref="UpdateCopilotUserInfoCommandHandler"/>.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public UpdateCopilotUserInfoCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    /// <summary>
    /// Handles a request of updating user info.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with no contents if the request completes successfully.</returns>
    /// <exception cref="PipelineException">Thrown when an internal error occurs, or the email is already in use.</exception>
    public async Task<MaaActionResult<EmptyObject>> Handle(UpdateCopilotUserInfoCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);

        if (user is null)
        {
            throw new PipelineException(MaaApiResponse.InternalError(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException));
        }

        if (string.IsNullOrEmpty(request.Email))
        {
            var exist = _dbContext.CopilotUsers.Any(x => x.Email == request.Email);
            if (exist)
            {
                throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(),
                    _apiErrorMessage.EmailAlreadyInUse));
            }
        }

        user.UpdateUserInfo(user.EntityId, request.Email, request.UserName);
        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
