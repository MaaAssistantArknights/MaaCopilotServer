// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

/// <summary>
///     Configuration of operations.
/// </summary>
public class CopilotOperationMap : IEntityTypeConfiguration<CopilotOperation>
{
    /// <summary>
    ///     Configures operation model.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CopilotOperation> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.RatingLevel)
            .HasConversion<EnumToStringConverter<RatingLevel>>();
        builder.Property(x => x.Operators)
            .HasConversion(
                list => string.Join(";", list),
                s => s.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList(),
                new ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        builder.Property(x => x.Groups)
            .HasConversion(
                list => string.Join(";", list),
                s => s.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList(),
                new ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }
}
