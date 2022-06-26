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
                    .Select(Guid.Parse).ToList(),
                new ValueComparer<List<Guid>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        builder.HasMany(x => x.Operations)
            .WithMany(x => x.Favorites)
            .UsingEntity<MapFavoriteOperation>(eb =>
            {
                eb.ToTable("Map_Favorite_Operation");
                eb.HasQueryFilter(x => !x.IsDeleted);

                eb.HasOne(x => x.Operations)
                    .WithMany()
                    .HasForeignKey(x => x.OperationsEntityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_FavOper_Oper_OperEntityId");
                eb.HasOne(x => x.Favorites)
                    .WithMany()
                    .HasForeignKey(x => x.FavoritesEntityId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_FavOper_Fav_FavEntityId");

                eb.Property(x => x.CreateBy)
                    .HasDefaultValue(Guid.Empty);
                eb.Property(x => x.DeleteBy)
                    .HasDefaultValue(Guid.Empty);
            });
        builder.HasQueryFilter(x => x.User.IsDeleted == false);
    }
}
