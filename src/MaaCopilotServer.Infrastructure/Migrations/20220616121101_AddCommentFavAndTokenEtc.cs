// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class AddCommentFavAndTokenEtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserRole",
                table: "CopilotUsers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "UserActivated",
                table: "CopilotUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CopilotUserFavoriteEntityId",
                table: "CopilotOperations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Favorites",
                table: "CopilotOperations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "CopilotOperations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Operators",
                table: "CopilotOperations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CopilotOperationComments",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    ReplyTo = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true)
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
                name: "CopilotTokens",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ValidBefore = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CopilotTokens", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "CopilotUserFavorites",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    FavoriteName = table.Column<string>(type: "text", nullable: false),
                    OperationGroupIds = table.Column<string>(type: "text", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_CopilotOperations_CopilotUserFavoriteEntityId",
                table: "CopilotOperations",
                column: "CopilotUserFavoriteEntityId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CopilotOperations_CopilotUserFavorites_CopilotUserFavoriteE~",
                table: "CopilotOperations",
                column: "CopilotUserFavoriteEntityId",
                principalTable: "CopilotUserFavorites",
                principalColumn: "EntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CopilotOperations_CopilotUserFavorites_CopilotUserFavoriteE~",
                table: "CopilotOperations");

            migrationBuilder.DropTable(
                name: "CopilotOperationComments");

            migrationBuilder.DropTable(
                name: "CopilotTokens");

            migrationBuilder.DropTable(
                name: "CopilotUserFavorites");

            migrationBuilder.DropIndex(
                name: "IX_CopilotOperations_CopilotUserFavoriteEntityId",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "UserActivated",
                table: "CopilotUsers");

            migrationBuilder.DropColumn(
                name: "CopilotUserFavoriteEntityId",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "Favorites",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "Operators",
                table: "CopilotOperations");

            migrationBuilder.AlterColumn<int>(
                name: "UserRole",
                table: "CopilotUsers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
