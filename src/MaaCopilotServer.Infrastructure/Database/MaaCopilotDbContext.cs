// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;
using MaaCopilotServer.Application.Common.Extensions;
using MaaCopilotServer.Application.Common.Interfaces;
using MaaCopilotServer.Domain.Common;
using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Options;
using MaaCopilotServer.Infrastructure.Database.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MaaCopilotServer.Infrastructure.Database;

/// <summary>
///     The DB context.
/// </summary>
public class MaaCopilotDbContext : DbContext, IMaaCopilotDbContext
{
    /// <summary>
    ///     The connection string.
    /// </summary>
    private readonly string? _connectionString;

    /// <summary>
    ///     The constructor with <see cref="IOptions{TOptions}"/>.
    /// </summary>
    /// <param name="dbOptions">The database options.</param>
    public MaaCopilotDbContext(IOptions<DatabaseOption> dbOptions)
    {
        _connectionString = dbOptions.Value.ConnectionString;
    }

    /// <inheritdoc />
    public DbSet<ArkI18N> ArkI18Ns { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<CopilotOperation> CopilotOperations { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<CopilotOperationRating> CopilotOperationRatings { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<CopilotUser> CopilotUsers { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<CopilotToken> CopilotTokens { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<PersistStorage> PersistStorage { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<ArkCharacterInfo> ArkCharacterInfos { get; set; } = null!;

    /// <inheritdoc />
    public DbSet<ArkLevelData> ArkLevelData { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Create Postgres database for development and production.
        optionsBuilder.UseNpgsql(_connectionString.IsNotNull());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyCommonConfigurations();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
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

    /// <summary>
    ///     Preparations before saving changes to DB.
    /// </summary>
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
