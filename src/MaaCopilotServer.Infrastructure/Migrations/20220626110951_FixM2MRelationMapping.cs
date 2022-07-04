// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class FixM2MRelationMapping : Migration
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

            migrationBuilder.RenameColumn(
                name: "Favorites",
                table: "CopilotOperations",
                newName: "FavoriteCount");

            migrationBuilder.CreateTable(
                name: "Map_Favorite_Operation",
                columns: table => new
                {
                    FavoritesEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationsEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true, defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map_Favorite_Operation", x => new { x.FavoritesEntityId, x.OperationsEntityId });
                    table.ForeignKey(
                        name: "FK_FavOper_Fav_FavEntityId",
                        column: x => x.FavoritesEntityId,
                        principalTable: "CopilotUserFavorites",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavOper_Oper_OperEntityId",
                        column: x => x.OperationsEntityId,
                        principalTable: "CopilotOperations",
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

            migrationBuilder.RenameColumn(
                name: "FavoriteCount",
                table: "CopilotOperations",
                newName: "Favorites");

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
