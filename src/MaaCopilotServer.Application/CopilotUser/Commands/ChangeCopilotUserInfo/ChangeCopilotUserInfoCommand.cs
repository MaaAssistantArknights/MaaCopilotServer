// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.ChangeCopilotUserInfo;

[Authorized(UserRole.Admin)]
public record ChangeCopilotUserInfoCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("user_id")] public string? UserId { get; set; }
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("user_name")] public string? UserName { get; set; }

    [JsonPropertyName("password")]
    [LogMasked]
    public string? Password { get; set; }

    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public string? Role { get; set; }
}

public class
    ChangeCopilotUserInfoCommandHandler : IRequestHandler<ChangeCopilotUserInfoCommand, MaaActionResult<EmptyObject>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;
    private readonly ApiErrorMessage _apiErrorMessage;

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

    public async Task<MaaActionResult<EmptyObject>> Handle(ChangeCopilotUserInfoCommand request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers
            .FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);

        if (user is null)
        {
            throw new PipelineException(MaaApiResponse.NotFound(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, request.UserId)));
        }

        var @operator = await _dbContext.CopilotUsers.FirstOrDefaultAsync(
            x => x.EntityId == _currentUserService.GetUserIdentity(), cancellationToken);
        if (@operator is null)
        {
            throw new PipelineException(MaaApiResponse.InternalError(_currentUserService.GetTrackingId(),
                _apiErrorMessage.InternalException));
        }

        if (@operator.UserRole is UserRole.Admin && user.UserRole >= UserRole.Admin)
        {
            throw new PipelineException(MaaApiResponse.Forbidden(_currentUserService.GetTrackingId(),
                _apiErrorMessage.PermissionDenied));
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
                throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(),
                    _apiErrorMessage.EmailAlreadyInUse));
            }
        }

        user.UpdateUserInfo(@operator.EntityId, request.Email, request.UserName, Enum.Parse<UserRole>(request.Role!));

        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
