// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;

/// <summary>
/// The record of creating user.
/// </summary>
[Authorized(UserRole.Admin)]
public record CreateCopilotUserCommand : IRequest<MaaActionResult<EmptyObject>>
{
    /// <summary>
    /// The user email.
    /// </summary>
    [JsonPropertyName("email")] public string? Email { get; set; }

    /// <summary>
    /// The user password.
    /// </summary>
    [JsonPropertyName("password")]
    [LogMasked]
    public string? Password { get; set; }

    /// <summary>
    /// The username.
    /// </summary>
    [JsonPropertyName("user_name")] public string? UserName { get; set; }

    /// <summary>
    /// The role of the user.
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}

/// <summary>
/// The handler of creating user.
/// </summary>
public class CreateCopilotUserCommandHandler : IRequestHandler<CreateCopilotUserCommand, MaaActionResult<EmptyObject>>
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
    /// The service for processing passwords and tokens.
    /// </summary>
    private readonly ISecretService _secretService;

    /// <summary>
    /// The constructor of <see cref="CreateCopilotUserCommandHandler"/>.
    /// </summary>
    /// <param name="dbContext">The DB context.</param>
    /// <param name="secretService">The service for processing passwords and tokens.</param>
    /// <param name="currentUserService">The service for current user.</param>
    /// <param name="apiErrorMessage">The API error message.</param>
    public CreateCopilotUserCommandHandler(
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
    /// Handles the request of creating user.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with no contents if the request completes successfully.</returns>
    /// <exception cref="PipelineException">Thrown when the email is already in use.</exception>
    public async Task<MaaActionResult<EmptyObject>> Handle(CreateCopilotUserCommand request,
        CancellationToken cancellationToken)
    {
        var emailColliding = await _dbContext.CopilotUsers.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailColliding)
        {
            throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(),
                _apiErrorMessage.EmailAlreadyInUse));
        }

        var hashedPassword = _secretService.HashPassword(request.Password!);
        var user = new Domain.Entities.CopilotUser(request.Email!, hashedPassword, request.UserName!,
            Enum.Parse<UserRole>(request.Role!), _currentUserService.GetUserIdentity()!.Value);
        _dbContext.CopilotUsers.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
