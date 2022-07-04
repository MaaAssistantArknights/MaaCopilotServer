// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class FixRatingTableIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CopilotOperationRating",
                table: "CopilotOperationRating");

            migrationBuilder.RenameTable(
                name: "CopilotOperationRating",
                newName: "CopilotOperationRatings");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreateAt",
                table: "CopilotOperationRatings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CopilotOperationRatings",
                table: "CopilotOperationRatings",
                column: "EntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CopilotOperationRatings",
                table: "CopilotOperationRatings");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "CopilotOperationRatings");

            migrationBuilder.RenameTable(
                name: "CopilotOperationRatings",
                newName: "CopilotOperationRating");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CopilotOperationRating",
                table: "CopilotOperationRating",
                column: "EntityId");
        }
    }
}
