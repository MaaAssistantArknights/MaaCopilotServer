using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class AddArknightsGameData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArkCharacterInfos",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    NameCn = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    NameJp = table.Column<string>(type: "text", nullable: false),
                    NameKo = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<string>(type: "text", nullable: true),
                    Profession = table.Column<string>(type: "text", nullable: false),
                    Star = table.Column<int>(type: "integer", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArkCharacterInfos", x => x.EntityId);
                });

            migrationBuilder.CreateTable(
                name: "ArkLevelData",
                columns: table => new
                {
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    NameCn = table.Column<string>(type: "text", nullable: false),
                    NameKo = table.Column<string>(type: "text", nullable: false),
                    NameJp = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    CatOneCn = table.Column<string>(type: "text", nullable: false),
                    CatOneKo = table.Column<string>(type: "text", nullable: false),
                    CatOneJp = table.Column<string>(type: "text", nullable: false),
                    CatOneEn = table.Column<string>(type: "text", nullable: false),
                    CatTwoCn = table.Column<string>(type: "text", nullable: false),
                    CatTwoKo = table.Column<string>(type: "text", nullable: false),
                    CatTwoJp = table.Column<string>(type: "text", nullable: false),
                    CatTwoEn = table.Column<string>(type: "text", nullable: false),
                    CatThreeCn = table.Column<string>(type: "text", nullable: false),
                    CatThreeKo = table.Column<string>(type: "text", nullable: false),
                    CatThreeJp = table.Column<string>(type: "text", nullable: false),
                    CatThreeEn = table.Column<string>(type: "text", nullable: false),
                    LevelId = table.Column<string>(type: "text", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArkLevelData", x => x.EntityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArkCharacterInfos");

            migrationBuilder.DropTable(
                name: "ArkLevelData");
        }
    }
}
