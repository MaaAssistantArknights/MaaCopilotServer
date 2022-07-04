// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class AddAuditFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreateBy",
                table: "CopilotUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteBy",
                table: "CopilotUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdateBy",
                table: "CopilotUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreateBy",
                table: "CopilotOperations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeleteBy",
                table: "CopilotOperations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdateBy",
                table: "CopilotOperations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "CopilotUsers");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "CopilotUsers");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "CopilotUsers");

            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "UpdateBy",
                table: "CopilotOperations");
        }
    }
}
