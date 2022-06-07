// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Exceptions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Security;
using MaaCopilotServer.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserPassword;

[Authorized(UserRole.User)]
public record UpdateCopilotUserPasswordCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("original_password")] public string? OriginalPassword { get; set; }

    [JsonPropertyName("new_password")] public string? NewPassword { get; set; }
}

public class UpdateCopilotUserPasswordCommandHandler : IRequestHandler<UpdateCopilotUserPasswordCommand,
    MaaActionResult<EmptyObject>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCopilotUserPasswordCommandHandler(
        IMaaCopilotDbContext dbContext,
        ISecretService secretService,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _secretService = secretService;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<EmptyObject>> Handle(UpdateCopilotUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == _currentUserService.GetUserIdentity()!.Value, cancellationToken);

        if (user is null)
        {
            throw new PipelineException(MaaApiResponse.InternalError(_currentUserService.GetTrackingId()));
        }

        var ok = _secretService.VerifyPassword(user!.Password, request.OriginalPassword!);

        if (ok is false)
        {
            return MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(), "Invalid password");
        }

        var hash = _secretService.HashPassword(request.NewPassword!);
        user.UpdatePassword(hash);
        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
