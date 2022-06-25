// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Application.Test.TestHelpers;

/// <summary>
///     Create a new in-memory database for each test.
/// </summary>
public class TestDbContext : MaaCopilotDbContext
{
    public TestDbContext() : base(new OptionsWrapper<DatabaseOption>(new DatabaseOption())) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }
}
