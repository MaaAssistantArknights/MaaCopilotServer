using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class UpdateOperationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StageName",
                table: "CopilotOperations");

            migrationBuilder.AddColumn<Guid>(
                name: "ArkLevelEntityId",
                table: "CopilotOperations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CopilotOperations_ArkLevelEntityId",
                table: "CopilotOperations",
                column: "ArkLevelEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CopilotOperations_ArkLevelData_ArkLevelEntityId",
                table: "CopilotOperations",
                column: "ArkLevelEntityId",
                principalTable: "ArkLevelData",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CopilotOperations_ArkLevelData_ArkLevelEntityId",
                table: "CopilotOperations");

            migrationBuilder.DropIndex(
                name: "IX_CopilotOperations_ArkLevelEntityId",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "ArkLevelEntityId",
                table: "CopilotOperations");

            migrationBuilder.AddColumn<string>(
                name: "StageName",
                table: "CopilotOperations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
