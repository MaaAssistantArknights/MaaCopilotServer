// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

[Authorized(UserRole.User)]
public record UpdateCopilotUserPasswordCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("original_password")]
    [NotLogged]
    public string? OriginalPassword { get; set; }

    [JsonPropertyName("new_password")]
    [NotLogged]
    public string? NewPassword { get; set; }
}

public class UpdateCopilotUserPasswordCommandHandler : IRequestHandler<UpdateCopilotUserPasswordCommand,
    MaaActionResult<EmptyObject>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;
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

    public async Task<MaaActionResult<EmptyObject>> Handle(UpdateCopilotUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == _currentUserService.GetUserIdentity()!.Value, cancellationToken);

        if (user is null)
        {
            throw new PipelineException(MaaApiResponse.InternalError(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException));
        }

        var ok = _secretService.VerifyPassword(user!.Password, request.OriginalPassword!);

        if (ok is false)
        {
            throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(),
                _apiErrorMessage.PasswordInvalid));
        }

        var hash = _secretService.HashPassword(request.NewPassword!);
        user.UpdatePassword(user.EntityId, hash);
        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
