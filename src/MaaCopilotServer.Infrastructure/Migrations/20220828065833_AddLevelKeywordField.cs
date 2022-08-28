using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class AddLevelKeywordField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KeywordEntityId",
                table: "ArkLevelData",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArkLevelData_KeywordEntityId",
                table: "ArkLevelData",
                column: "KeywordEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_KeywordEntityId",
                table: "ArkLevelData",
                column: "KeywordEntityId",
                principalTable: "ArkI18Ns",
                principalColumn: "EntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_KeywordEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropIndex(
                name: "IX_ArkLevelData_KeywordEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "KeywordEntityId",
                table: "ArkLevelData");
        }
    }
}
