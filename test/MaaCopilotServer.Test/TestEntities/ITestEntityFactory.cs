// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Common;

namespace MaaCopilotServer.Test.TestEntities;

/// <summary>
/// The factory class of entity.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public interface ITestEntityFactory<TEntity>
    where TEntity : BaseEntity
{
    /// <summary>
    /// Builds entity.
    /// </summary>
    /// <returns>The entity built.</returns>
    TEntity Build();
}
