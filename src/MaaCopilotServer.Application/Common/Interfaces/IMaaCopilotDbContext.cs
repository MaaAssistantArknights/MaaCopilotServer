// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
/// The DB context.
/// </summary>
public interface IMaaCopilotDbContext
{
    /// <summary>
    /// The DB set of operations.
    /// </summary>
    DbSet<Domain.Entities.CopilotOperation> CopilotOperations { get; }

    /// <summary>
    /// The DB set of users.
    /// </summary>
    DbSet<Domain.Entities.CopilotUser> CopilotUsers { get; }
    DbSet<Domain.Entities.CopilotToken> CopilotTokens { get; }
    DbSet<Domain.Entities.CopilotOperationComment> CopilotOperationComments { get; }
    DbSet<Domain.Entities.CopilotUserFavorite> CopilotUserFavorites { get; }

    /// <summary>
    /// Saves changes to DB asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
