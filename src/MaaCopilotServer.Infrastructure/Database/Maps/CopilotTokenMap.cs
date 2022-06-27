// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

public class CopilotTokenMap : IEntityTypeConfiguration<CopilotToken>
{
    public void Configure(EntityTypeBuilder<CopilotToken> builder)
    {
        builder.Property(x => x.Type)
            .HasConversion<EnumToStringConverter<TokenType>>();
    }
}
