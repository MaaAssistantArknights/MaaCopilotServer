// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.UpdateCopilotUserInfo;

[Authorized(UserRole.User)]
public record UpdateCopilotUserInfoCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("user_name")] public string? UserName { get; set; }
}

public class
    UpdateCopilotUserInfoCommandHandler : IRequestHandler<UpdateCopilotUserInfoCommand, MaaActionResult<EmptyObject>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;

    public UpdateCopilotUserInfoCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

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
