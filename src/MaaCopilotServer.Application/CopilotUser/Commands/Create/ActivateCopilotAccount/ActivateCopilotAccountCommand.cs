// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.ActivateCopilotAccount;

public record ActivateCopilotAccountCommand : IRequest<MaaApiResponse<GetCopilotUserDto>>
{
    /// <summary>
    ///     Account activation token.
    /// </summary>
    [JsonPropertyName("token")]
    public string? Token { get; set; }
}

public class
    ActivateCopilotAccountCommandHandler : IRequestHandler<ActivateCopilotAccountCommand, MaaApiResponse<GetCopilotUserDto>>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public ActivateCopilotAccountCommandHandler(
        ICurrentUserService currentUserService,
        IMaaCopilotDbContext dbContext,
        ApiErrorMessage apiErrorMessage)
    {
        _currentUserService = currentUserService;
        _dbContext = dbContext;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse<GetCopilotUserDto>> Handle(ActivateCopilotAccountCommand request,
        CancellationToken cancellationToken)
    {
        var token = await _dbContext.CopilotTokens.FirstOrDefaultAsync(x => x.Token == request.Token,
            cancellationToken);
        if (token is null || token.ValidBefore < DateTimeOffset.UtcNow || token.Type != TokenType.UserActivation)
        {
            throw new PipelineException(MaaApiResponseHelper.BadRequest(_currentUserService.GetTrackingId(),
                _apiErrorMessage.TokenInvalid));
        }

        var user = _dbContext.CopilotUsers.FirstOrDefault(x => x.EntityId == token.ResourceId);
        if (user is null)
        {
            throw new PipelineException(MaaApiResponseHelper.InternalError(_currentUserService.GetTrackingId(),
                string.Format(_apiErrorMessage.UserWithIdNotFound!, token.ResourceId.ToString())));
        }

        user.ActivateUser(user.EntityId);
        token.Delete(user.EntityId);
        _dbContext.CopilotUsers.Update(user);
        _dbContext.CopilotTokens.Remove(token);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return MaaApiResponseHelper.Ok<GetCopilotUserDto>(null, _currentUserService.GetTrackingId());
    }
}
