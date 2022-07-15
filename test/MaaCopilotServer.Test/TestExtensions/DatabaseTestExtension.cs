// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;

namespace MaaCopilotServer.Test.TestExtensions;

/// <summary>
/// Test extensions for <see cref="IMaaCopilotDbContext"/>.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DatabaseTestExtension
{
    /// <summary>
    /// Setups mock <see cref="IMaaCopilotDbContext"/>.
    /// </summary>
    /// <param name="db">The database context.</param>
    /// <param name="action">The actions to setup.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="db"/> or <paramref name="action"/> is <c>null</c>.</exception>
    public static void Setup(this IMaaCopilotDbContext db, Action<IMaaCopilotDbContext> action)
    {
        if (db == null)
        {
            throw new ArgumentNullException(nameof(db));
        }

        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        action.Invoke(db);
        db.SaveChangesAsync(default).Wait();
    }
}
