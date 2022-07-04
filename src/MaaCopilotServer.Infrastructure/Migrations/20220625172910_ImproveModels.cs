// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class ImproveModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "CopilotOperations");

            migrationBuilder.RenameColumn(
                name: "OperationGroupIds",
                table: "CopilotUserFavorites",
                newName: "OperationIds");

            migrationBuilder.RenameColumn(
                name: "Downloads",
                table: "CopilotOperations",
                newName: "ViewCounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OperationIds",
                table: "CopilotUserFavorites",
                newName: "OperationGroupIds");

            migrationBuilder.RenameColumn(
                name: "ViewCounts",
                table: "CopilotOperations",
                newName: "Downloads");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "CopilotOperations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
