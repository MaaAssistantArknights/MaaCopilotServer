// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class AddHotScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CopilotOperationComments");

            migrationBuilder.DropTable(
                name: "Map_Favorite_Operation");

            migrationBuilder.DropTable(
                name: "CopilotUserFavorites");

            migrationBuilder.DropColumn(
                name: "FavoriteCount",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "RatingRatio",
                table: "CopilotOperations");

            migrationBuilder.AddColumn<long>(
                name: "HotScore",
                table: "CopilotOperations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "RatingLevel",
                table: "CopilotOperations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotScore",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "RatingLevel",
                table: "CopilotOperations");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteCount",
                table: "CopilotOperations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "RatingRatio",
                table: "CopilotOperations",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "CopilotOperationComments",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ReplyTo = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CopilotOperationComments", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_CopilotOperationComments_CopilotOperations_OperationEntityId",
                        column: x => x.OperationEntityId,
                        principalTable: "CopilotOperations",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CopilotOperationComments_CopilotUsers_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "CopilotUsers",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CopilotUserFavorites",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true),
                    FavoriteName = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    OperationIds = table.Column<string>(type: "text", nullable: false),
                    UpdateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CopilotUserFavorites", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_CopilotUserFavorites_CopilotUsers_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "CopilotUsers",
                        principalColumn: "EntityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Map_Favorite_Operation",
                columns: table => new
                {
                    FavoritesEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationsEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
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
                name: "IX_CopilotOperationComments_OperationEntityId",
                table: "CopilotOperationComments",
                column: "OperationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CopilotOperationComments_UserEntityId",
                table: "CopilotOperationComments",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CopilotUserFavorites_UserEntityId",
                table: "CopilotUserFavorites",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Map_Favorite_Operation_OperationsEntityId",
                table: "Map_Favorite_Operation",
                column: "OperationsEntityId");
        }
    }
}
