// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using Destructurama.Attributed;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.PasswordReset;

public record PasswordResetCommand : IRequest<MaaApiResponse<GetCopilotUserDto>>
{
    [JsonPropertyName("token")] public string? Token { get; set; }

    [JsonPropertyName("password")]
    [LogMasked]
    public string? Password { get; set; }
}

public class PasswordResetCommandHandler :
    IRequestHandler<PasswordResetCommand, MaaApiResponse<GetCopilotUserDto>>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;

    public PasswordResetCommandHandler(
        ICurrentUserService currentUserService,
        ISecretService secretService,
        IMaaCopilotDbContext dbContext,
        ApiErrorMessage apiErrorMessage)
    {
        _currentUserService = currentUserService;
        _secretService = secretService;
        _dbContext = dbContext;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse<GetCopilotUserDto>> Handle(PasswordResetCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _dbContext.CopilotTokens.FirstOrDefaultAsync(x => x.Token == request.Token,
            cancellationToken);
        if (token is null || token.ValidBefore < DateTimeOffset.UtcNow || token.Type != TokenType.UserPasswordReset)
        {
            return MaaApiResponseHelper.BadRequest<GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                _apiErrorMessage.TokenInvalid);
        }

        var user = _dbContext.CopilotUsers.FirstOrDefault(x => x.EntityId == token.ResourceId);
        if (user is null)
        {
            return MaaApiResponseHelper.InternalError<GetCopilotUserDto>(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, token.ResourceId.ToString()));
        }

        user.UpdatePassword(user.EntityId, _secretService.HashPassword(request.Password!));

        _dbContext.CopilotUsers.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok<GetCopilotUserDto>(null, _currentUserService.GetTrackingId());
    }
}
