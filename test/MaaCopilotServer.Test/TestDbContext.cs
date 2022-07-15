// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Test;

/// <summary>
///     Create a new in-memory database for each test.
/// </summary>
[ExcludeFromCodeCoverage]
public class TestDbContext : MaaCopilotDbContext
{
    /// <summary>
    /// The constructor.
    /// </summary>
    public TestDbContext() : base(new OptionsWrapper<DatabaseOption>(new DatabaseOption())) { }

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
}
