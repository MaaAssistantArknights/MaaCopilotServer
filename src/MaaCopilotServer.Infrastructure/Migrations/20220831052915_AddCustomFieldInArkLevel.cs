using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class AddCustomFieldInArkLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Custom",
                table: "ArkLevelData",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Custom",
                table: "ArkLevelData");
        }
    }
}
