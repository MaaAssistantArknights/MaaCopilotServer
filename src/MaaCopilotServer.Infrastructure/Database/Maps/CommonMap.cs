// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;
using MaaCopilotServer.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

/// <summary>
///     The extension for common configurations.
/// </summary>
public static class CommonMap
{
    /// <summary>
    ///     Applies common configurations.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    public static void ApplyCommonConfigurations(this ModelBuilder modelBuilder)
    {
        var method = typeof(CommonMap).GetTypeInfo().DeclaredMethods
            .Single(m => m.Name == nameof(Configure));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsSubclassOf(typeof(BaseEntity)))
            {
                method.MakeGenericMethod(entityType.ClrType).Invoke(null, new object?[] { modelBuilder });
            }
        }
    }

    /// <summary>
    ///     Configures entity type model.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <param name="modelBuilder">The module builder.</param>
    private static void Configure<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity
    {
        modelBuilder.Entity<TEntity>(builder =>
        {
            builder.HasQueryFilter(p => !p.IsDeleted);
            builder.Property(p => p.EntityId).ValueGeneratedNever();
            builder.Property(p => p.CreateAt).ValueGeneratedNever();
        });
    }
}
