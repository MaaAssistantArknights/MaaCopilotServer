// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Common.Interfaces;

public interface IMaaCopilotDbContext
{
    DbSet<Domain.Entities.CopilotOperation> CopilotOperations { get; }
    DbSet<Domain.Entities.CopilotUser> CopilotUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
