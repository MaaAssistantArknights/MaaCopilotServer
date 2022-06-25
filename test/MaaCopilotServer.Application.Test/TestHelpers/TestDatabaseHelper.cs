// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Application.Test.TestHelpers;

/// <summary>
/// The helper class to create in-memory database for testing.
/// </summary>
public static class TestDatabaseHelper
{
    /// <summary>
    /// Gets in-memory database context.
    /// </summary>
    /// <returns>The database context.</returns>
    public static IMaaCopilotDbContext GetTestDbContext()
    {
        var options = new DbContextOptionsBuilder<MaaCopilotDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;


        var db = new MaaCopilotDbContext(options);
        return db;
    }
}
