// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

/// <summary>
///     The DTO for the UpdateCopilotUserPassword command.
/// </summary>
[Authorized(UserRole.User, true)]
public record UpdateCopilotUserPasswordCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The original password.
    /// </summary>
    [JsonPropertyName("original_password")]
    [NotLogged]
    [Required]
    public string? OriginalPassword { get; set; }

    /// <summary>
    ///     The new password.
    /// </summary>
    [JsonPropertyName("new_password")]
    [NotLogged]
    [Required]
    public string? NewPassword { get; set; }
}

public class UpdateCopilotUserPasswordCommandHandler : IRequestHandler<UpdateCopilotUserPasswordCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;

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

    public async Task<MaaApiResponse> Handle(UpdateCopilotUserPasswordCommand request, CancellationToken cancellationToken)
    {
        // Get current infos.
        var user = (await _currentUserService.GetUser()).IsNotNull();

        // Check if original password is valid or not
        var ok = _secretService.VerifyPassword(user.Password, request.OriginalPassword!);
        if (ok is false)
        {
            return MaaApiResponseHelper.BadRequest(_apiErrorMessage.PasswordInvalid);
        }

        // Set new password
        var hash = _secretService.HashPassword(request.NewPassword!);
        user.UpdatePassword(user.EntityId, hash);
        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
