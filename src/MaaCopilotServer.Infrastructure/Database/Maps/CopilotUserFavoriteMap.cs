// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

public class CopilotUserFavoriteMap : IEntityTypeConfiguration<CopilotUserFavorite>
{
    public void Configure(EntityTypeBuilder<CopilotUserFavorite> builder)
    {
        builder.Property(x => x.OperationIds)
            .HasConversion(
                list => string.Join(";", list.Select(x => x.ToString())),
                s => s.Split(";", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Guid.Parse(s)).ToList(),
                new ValueComparer<List<Guid>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        builder.HasMany(x => x.Operations)
            .WithMany(x => x.FavoriteBy)
            .UsingEntity(eb => eb.ToTable("Map_Favorite_Operation"));
        builder.HasQueryFilter(x => x.User.IsDeleted == false);
    }
}
