// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MaaCopilotServer.Application.Common.Helpers;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.CopilotUser.Commands.DeleteCopilotUser;

/// <summary>
///     The DTO for the DeleteCopilotUser command.
/// </summary>
[Authorized(UserRole.Admin)]
public record DeleteCopilotUserCommand : IRequest<MaaApiResponse>
{
    /// <summary>
    ///     The id of the user that pending to delete.
    /// </summary>
    [Required]
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}

public class DeleteCopilotUserCommandHandler : IRequestHandler<DeleteCopilotUserCommand, MaaApiResponse>
{
    private readonly ApiErrorMessage _apiErrorMessage;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMaaCopilotDbContext _dbContext;

    public DeleteCopilotUserCommandHandler(
        IMaaCopilotDbContext dbContext,
        ICurrentUserService currentUserService,
        ApiErrorMessage apiErrorMessage)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _apiErrorMessage = apiErrorMessage;
    }

    public async Task<MaaApiResponse> Handle(DeleteCopilotUserCommand request, CancellationToken cancellationToken)
    {
        // Get requested user data
        var userId = Guid.Parse(request.UserId!);
        var user = await _dbContext.CopilotUsers.FirstOrDefaultAsync(x => x.EntityId == userId, cancellationToken);
        if (user == null)
        {
            return MaaApiResponseHelper.NotFound(
                string.Format(_apiErrorMessage.UserWithIdNotFound!, userId));
        }

        // Get operator
        var @operator = (await _currentUserService.GetUser()).IsNotNull();

        // The operator's role is Above or Equal to Admin
        // He can not delete any user who have the role Above or Equal to himself.
        // So an Admin could not delete an Admin account include himself and SuperAdmin account.
        // Only SuperAdmin could delete Admin accounts.
        // But this also means, SuperAdmin account could not be deleted.
        if (@operator.UserRole <= user.UserRole)
        {
            return MaaApiResponseHelper.Forbidden(_apiErrorMessage.PermissionDenied);
        }

        // Delete user
        user.Delete(@operator.EntityId);
        _dbContext.CopilotUsers.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return MaaApiResponseHelper.Ok();
    }
}
