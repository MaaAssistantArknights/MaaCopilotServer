// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MaaCopilotServer.Application.Common.Interfaces;

/// <summary>
///     The DB context.
/// </summary>
public interface IMaaCopilotDbContext
{
    /// <summary>
    ///     The DB set of ark i18n strings.
    /// </summary>
    DbSet<ArkI18N> ArkI18Ns { get; }

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
    ///     The DB set of some system persistent data.
    /// </summary>
    DbSet<PersistStorage> PersistStorage { get; }

    /// <summary>
    ///     The DB set of arknights characters.
    /// </summary>
    DbSet<ArkCharacterInfo> ArkCharacterInfos { get; }

    /// <summary>
    ///     The DB set of arknights levels.
    /// </summary>
    DbSet<ArkLevelData> ArkLevelData { get; }

    /// <summary>
    ///     Saves changes to DB asynchronously.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task with the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// See <see cref="DbContext.Update{TEntity}(TEntity)"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity to update.</param>
    /// <returns>
    /// The <see cref="EntityEntry{TEntity}" /> for the entity. The entry provides
    /// access to change tracking information and operations for the entity.
    /// </returns>
    EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        where TEntity : class;
}
