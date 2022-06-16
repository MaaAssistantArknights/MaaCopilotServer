// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Entities;

namespace MaaCopilotServer.Infrastructure.Services;

/// <summary>
/// The service for getting user info.
/// </summary>
public class IdentityService : IIdentityService
{
    /// <summary>
    /// The DB context.
    /// </summary>
    private readonly IMaaCopilotDbContext _copilotDbContext;

    /// <summary>
    /// The constructor of <see cref="IdentityService"/>.
    /// </summary>
    /// <param name="copilotDbContext">The DB context.</param>
    public IdentityService(IMaaCopilotDbContext copilotDbContext)
    {
        _copilotDbContext = copilotDbContext;
    }

    /// <summary>
    /// Gets user info asynchronously.
    /// </summary>
    /// <param name="guid">The user ID.</param>
    /// <returns>A task with the user info.</returns>
    public async Task<CopilotUser?> GetUserAsync(Guid guid)
    {
        var user = await _copilotDbContext.CopilotUsers.FindAsync(guid);
        return user;
    }
}
