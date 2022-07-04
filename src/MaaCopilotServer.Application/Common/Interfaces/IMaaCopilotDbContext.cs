// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
///     The DB context.
/// </summary>
public interface IMaaCopilotDbContext
{
    /// <summary>
    ///     The DB set of operations.
    /// </summary>
    DbSet<Domain.Entities.CopilotOperation> CopilotOperations { get; }

    /// <summary>
    ///     The DB set of users.
    /// </summary>
    DbSet<Domain.Entities.CopilotUser> CopilotUsers { get; }

    /// <summary>
    ///     The DB set of tokens.
    /// </summary>
    DbSet<CopilotToken> CopilotTokens { get; }

    /// <summary>
    ///     The DB set of operation rating.
    /// </summary>
    DbSet<CopilotOperationRating> CopilotOperationRatings { get; }

    /// <summary>
    ///     Saves changes to DB asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
