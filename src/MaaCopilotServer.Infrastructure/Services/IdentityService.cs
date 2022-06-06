// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Entities;

namespace MaaCopilotServer.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IMaaCopilotDbContext _copilotDbContext;

    public IdentityService(IMaaCopilotDbContext copilotDbContext)
    {
        _copilotDbContext = copilotDbContext;
    }

    public async Task<CopilotUser?> GetUserAsync(Guid guid)
    {
        var user = await _copilotDbContext.CopilotUsers.FindAsync(guid);
        return user;
    }
}
