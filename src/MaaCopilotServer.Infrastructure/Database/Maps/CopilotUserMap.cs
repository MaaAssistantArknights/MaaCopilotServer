// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using MaaCopilotServer.Domain.Entities;
using MaaCopilotServer.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MaaCopilotServer.Infrastructure.Database.Maps;

public class CopilotUserMap : IEntityTypeConfiguration<CopilotUser>
{
    public void Configure(EntityTypeBuilder<CopilotUser> builder)
    {
        builder.Property(x => x.UserRole).HasConversion<EnumToStringConverter<UserRole>>();
    }
}
