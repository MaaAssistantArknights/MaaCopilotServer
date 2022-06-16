// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

/// <summary>
/// Configuration of operations.
/// </summary>
public class CopilotOperationMap : IEntityTypeConfiguration<CopilotOperation>
{
    /// <summary>
    /// Configures operation model.
    /// </summary>
    /// <param name="builder">The entity type builder.</param>
    public void Configure(EntityTypeBuilder<CopilotOperation> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
