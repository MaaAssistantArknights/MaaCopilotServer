// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;

/// <summary>
///     The DTO for the CreateCopilotUser command.
/// </summary>
[Authorized(UserRole.Admin)]
public record CreateCopilotUserCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The user email.
    /// </summary>
    [Required]
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    ///     The user password.
    /// </summary>
    [JsonPropertyName("password")]
    [LogMasked]
    [Required]
    public string? Password { get; set; }

    /// <summary>
    ///     The username.
    /// </summary>
    [Required]
    [JsonPropertyName("user_name")]
    public string? UserName { get; set; }

    /// <summary>
    ///     The role of the user. Valid values are: Admin, Uploader, User.
    /// </summary>
    [Required]
    [JsonPropertyName("role")]
    public string? Role { get; set; }
}

public class CreateCopilotUserCommandHandler : IRequestHandler<CreateCopilotUserCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;

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

    public async Task<MaaApiResponse> Handle(CreateCopilotUserCommand request, CancellationToken cancellationToken)
    {
        // Check if the email has already been used or not
        var emailColliding = await _dbContext.CopilotUsers.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailColliding)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.EmailAlreadyInUse);
        }

        // Hash password and build entity
        var hashedPassword = _secretService.HashPassword(request.Password!);
        var user = new Domain.Entities.CopilotUser(request.Email!, hashedPassword, request.UserName!,
            Enum.Parse<UserRole>(request.Role!), _currentUserService.GetUserIdentity()!.Value);

        // User account created by the Admin will be activated by default
        user.ActivateUser(_currentUserService.GetUserIdentity()!.Value);

        // Add user
        _dbContext.CopilotUsers.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
