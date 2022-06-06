// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Infrastructure.Database.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MaaCopilotServer.Infrastructure.Database;

public class MaaCopilotDbContext : DbContext, IMaaCopilotDbContext
{
    public DbSet<CopilotOperation> CopilotOperations { get; set; } = null!;
    public DbSet<CopilotUser> CopilotUsers { get; set; } = null!;

    private string? _connectionString;

    public MaaCopilotDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetValue<string>("Database:ConnectionString");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _connectionString ??= Environment.GetEnvironmentVariable("Database_ConnectionString");
        optionsBuilder.UseNpgsql(_connectionString!);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyCommonConfigurations();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Added or EntityState.Deleted)
            .ToList();
        foreach (var entry in entities)
        {
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (entry.State)
            {
                case EntityState.Added:
                    ((BaseEntity)entry.Entity).IsDeleted = false;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    ((BaseEntity)entry.Entity).IsDeleted = true;
                    break;
            }
        }
    }
}
