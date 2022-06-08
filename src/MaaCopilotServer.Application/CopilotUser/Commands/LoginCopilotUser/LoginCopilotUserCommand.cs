// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using Destructurama.Attributed;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.LoginCopilotUser;

public record LoginCopilotUserCommand : IRequest<MaaActionResult<LoginCopilotUserDto>>
{
    [JsonPropertyName("email")] public string? Email { get; set; }

    [JsonPropertyName("password")]
    [LogMasked]
    public string? Password { get; set; }
}

public class
    LoginCopilotUserCommandHandler : IRequestHandler<LoginCopilotUserCommand, MaaActionResult<LoginCopilotUserDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;

    public LoginCopilotUserCommandHandler(
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

    public async Task<MaaActionResult<LoginCopilotUserDto>> Handle(LoginCopilotUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (user is null)
        {
            throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(), _apiErrorMessage.LoginFailed));
        }

        var ok = _secretService.VerifyPassword(user.Password, request.Password!);
        if (ok is false)
        {
            throw new PipelineException(MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(),
                _apiErrorMessage.LoginFailed));
        }

        var (token, expire) = _secretService.GenerateJwtToken(user.EntityId);
        var dto = new LoginCopilotUserDto(token, expire.ToStringZhHans(), user.UserName);
        return MaaApiResponse.Ok(dto, _currentUserService.GetTrackingId());
    }
}
