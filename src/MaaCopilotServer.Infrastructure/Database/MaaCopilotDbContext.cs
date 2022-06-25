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
    ///     <para>Whether the database is created for unit test or not.</para>
    ///     <para>When in test mode, an in-memory SQLite database is created.</para>
    ///     <para>
    ///         Otherwise, the default PostGreSQL database is created
    ///         with <see cref="_connectionString"/>.
    ///     </para>
    /// </summary>
    private readonly bool _isTestMode = false;

    /// <summary>
    ///     The constructor with <see cref="IOptions{TOptions}"/>.
    /// </summary>
    /// <param name="dbOptions">The database options.</param>
    public MaaCopilotDbContext(IOptions<DatabaseOption> dbOptions)
    {
        _connectionString = dbOptions.Value.ConnectionString;
    }

    /// <summary>
    ///     The constructor with <see cref="DbContextOptions"/>.
    /// </summary>
    /// <param name="options"></param>
    /// <remarks>
    ///     <para>
    ///         This constructor is to be used only with unit tests.
    ///         It is supposed to create an temporary in-memory database
    ///         only for testing, and will be disposed after the test exits.
    ///     </para>
    ///     <para>
    ///         For development and production, please use
    ///         <see cref="MaaCopilotDbContext(IOptions{DatabaseOption})"/>
    ///         instead.
    ///     </para>
    /// </remarks>
    public MaaCopilotDbContext(DbContextOptions options)
        : base(options)
    {
        _isTestMode = true;
    }

    /// <summary>
    ///     The DB set of operations.
    /// </summary>
    public DbSet<CopilotOperation> CopilotOperations { get; set; } = null!;

    public DbSet<CopilotOperationComment> CopilotOperationComments { get; set; } = null!;
    public DbSet<CopilotUserFavorite> CopilotUserFavorites { get; set; } = null!;

    /// <summary>
    ///     The DB set of users.
    /// </summary>
    public DbSet<CopilotUser> CopilotUsers { get; set; } = null!;

    public DbSet<CopilotToken> CopilotTokens { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Create in-memory database when in test mode.
        if (_isTestMode)
        {
            base.OnConfiguring(optionsBuilder);
            return;
        }

        // Create PostgreSQL database for development and production.
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
