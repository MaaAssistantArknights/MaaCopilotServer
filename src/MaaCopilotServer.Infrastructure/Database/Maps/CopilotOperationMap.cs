// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

public class CopilotOperationMap : IEntityTypeConfiguration<CopilotOperation>
{
    public void Configure(EntityTypeBuilder<CopilotOperation> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
