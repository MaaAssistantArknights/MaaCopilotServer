// This file is a part of MaaCopilotServer project.
// MaaCopilotServer belongs to the MAA organization.
// Licensed under the AGPL-3.0 license.

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class NewGameDataSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CatOneCn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatOneEn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatOneJp",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatOneKo",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatThreeCn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatThreeEn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatThreeJp",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatThreeKo",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatTwoCn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatTwoEn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatTwoJp",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatTwoKo",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "NameCn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "NameJp",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "NameKo",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "NameCn",
                table: "ArkCharacterInfos");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "ArkCharacterInfos");

            migrationBuilder.DropColumn(
                name: "NameJp",
                table: "ArkCharacterInfos");

            migrationBuilder.DropColumn(
                name: "NameKo",
                table: "ArkCharacterInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "CatOneEntityId",
                table: "ArkLevelData",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CatThreeEntityId",
                table: "ArkLevelData",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CatTwoEntityId",
                table: "ArkLevelData",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "NameEntityId",
                table: "ArkLevelData",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "NameEntityId",
                table: "ArkCharacterInfos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ArkI18Ns",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChineseSimplified = table.Column<string>(type: "text", nullable: false),
                    ChineseTraditional = table.Column<string>(type: "text", nullable: false),
                    English = table.Column<string>(type: "text", nullable: false),
                    Japanese = table.Column<string>(type: "text", nullable: false),
                    Korean = table.Column<string>(type: "text", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArkI18Ns", x => x.EntityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArkLevelData_CatOneEntityId",
                table: "ArkLevelData",
                column: "CatOneEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ArkLevelData_CatThreeEntityId",
                table: "ArkLevelData",
                column: "CatThreeEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ArkLevelData_CatTwoEntityId",
                table: "ArkLevelData",
                column: "CatTwoEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ArkLevelData_NameEntityId",
                table: "ArkLevelData",
                column: "NameEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ArkCharacterInfos_NameEntityId",
                table: "ArkCharacterInfos",
                column: "NameEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArkCharacterInfos_ArkI18Ns_NameEntityId",
                table: "ArkCharacterInfos",
                column: "NameEntityId",
                principalTable: "ArkI18Ns",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_CatOneEntityId",
                table: "ArkLevelData",
                column: "CatOneEntityId",
                principalTable: "ArkI18Ns",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_CatThreeEntityId",
                table: "ArkLevelData",
                column: "CatThreeEntityId",
                principalTable: "ArkI18Ns",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_CatTwoEntityId",
                table: "ArkLevelData",
                column: "CatTwoEntityId",
                principalTable: "ArkI18Ns",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_NameEntityId",
                table: "ArkLevelData",
                column: "NameEntityId",
                principalTable: "ArkI18Ns",
                principalColumn: "EntityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArkCharacterInfos_ArkI18Ns_NameEntityId",
                table: "ArkCharacterInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_CatOneEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_CatThreeEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_CatTwoEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropForeignKey(
                name: "FK_ArkLevelData_ArkI18Ns_NameEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropTable(
                name: "ArkI18Ns");

            migrationBuilder.DropIndex(
                name: "IX_ArkLevelData_CatOneEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropIndex(
                name: "IX_ArkLevelData_CatThreeEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropIndex(
                name: "IX_ArkLevelData_CatTwoEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropIndex(
                name: "IX_ArkLevelData_NameEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropIndex(
                name: "IX_ArkCharacterInfos_NameEntityId",
                table: "ArkCharacterInfos");

            migrationBuilder.DropColumn(
                name: "CatOneEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatThreeEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "CatTwoEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "NameEntityId",
                table: "ArkLevelData");

            migrationBuilder.DropColumn(
                name: "NameEntityId",
                table: "ArkCharacterInfos");

            migrationBuilder.AddColumn<string>(
                name: "CatOneCn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatOneEn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatOneJp",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatOneKo",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatThreeCn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatThreeEn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatThreeJp",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatThreeKo",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatTwoCn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatTwoEn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatTwoJp",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CatTwoKo",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameCn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameJp",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameKo",
                table: "ArkLevelData",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameCn",
                table: "ArkCharacterInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "ArkCharacterInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameJp",
                table: "ArkCharacterInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameKo",
                table: "ArkCharacterInfos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
