// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System.Reflection;
using MaaCopilotServer.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

public static class CommonMap
{
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
