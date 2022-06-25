using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class ImproveM2MRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CopilotOperations_CopilotUserFavorites_CopilotUserFavoriteE~",
                table: "CopilotOperations");

            migrationBuilder.DropIndex(
                name: "IX_CopilotOperations_CopilotUserFavoriteEntityId",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "CopilotUserFavoriteEntityId",
                table: "CopilotOperations");

            migrationBuilder.CreateTable(
                name: "Map_Favorite_Operation",
                columns: table => new
                {
                    FavoriteByEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationsEntityId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map_Favorite_Operation", x => new { x.FavoriteByEntityId, x.OperationsEntityId });
                    table.ForeignKey(
                        name: "FK_Map_Favorite_Operation_CopilotOperations_OperationsEntityId",
                        column: x => x.OperationsEntityId,
                        principalTable: "CopilotOperations",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Map_Favorite_Operation_CopilotUserFavorites_FavoriteByEntit~",
                        column: x => x.FavoriteByEntityId,
                        principalTable: "CopilotUserFavorites",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Map_Favorite_Operation_OperationsEntityId",
                table: "Map_Favorite_Operation",
                column: "OperationsEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Map_Favorite_Operation");

            migrationBuilder.AddColumn<Guid>(
                name: "CopilotUserFavoriteEntityId",
                table: "CopilotOperations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CopilotOperations_CopilotUserFavoriteEntityId",
                table: "CopilotOperations",
                column: "CopilotUserFavoriteEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CopilotOperations_CopilotUserFavorites_CopilotUserFavoriteE~",
                table: "CopilotOperations",
                column: "CopilotUserFavoriteEntityId",
                principalTable: "CopilotUserFavorites",
                principalColumn: "EntityId");
        }
    }
}
