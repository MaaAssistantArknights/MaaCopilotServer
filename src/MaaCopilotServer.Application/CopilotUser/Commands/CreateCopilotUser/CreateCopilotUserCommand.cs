// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Application.Common.Models;
using MaaCopilotServer.Application.Common.Security;
using MaaCopilotServer.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.CreateCopilotUser;

[Authorized(UserRole.Admin)]
public record CreateCopilotUserCommand : IRequest<MaaActionResult<EmptyObject>>
{
    [JsonPropertyName("email")] public string? Email { get; set; }
    [JsonPropertyName("password")] public string? Password { get; set; }
    [JsonPropertyName("user_name")] public string? UserName { get; set; }
    [JsonPropertyName("role"), JsonConverter(typeof(JsonStringEnumConverter))] public UserRole? Role { get; set; }
}

public class CreateCopilotUserCommandHandler : IRequestHandler<CreateCopilotUserCommand, MaaActionResult<EmptyObject>>
{
    private readonly IMaaCopilotDbContext _dbContext;
    private readonly ISecretService _secretService;
    private readonly ICurrentUserService _currentUserService;

    public CreateCopilotUserCommandHandler(
        IMaaCopilotDbContext dbContext,
        ISecretService secretService,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _secretService = secretService;
        _currentUserService = currentUserService;
    }

    public async Task<MaaActionResult<EmptyObject>> Handle(CreateCopilotUserCommand request, CancellationToken cancellationToken)
    {
        var emailColliding = await _dbContext.CopilotUsers.AnyAsync(x => x.Email == request.Email, cancellationToken);
        if (emailColliding)
        {
            return MaaApiResponse.BadRequest(_currentUserService.GetTrackingId(), "Email already in use");
        }

        var hashedPassword = _secretService.HashPassword(request.Password!);
        var user = new Domain.Entities.CopilotUser(request.Email!, hashedPassword, request.UserName!, request.Role!.Value);
        _dbContext.CopilotUsers.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponse.Ok(null, _currentUserService.GetTrackingId());
    }
}
