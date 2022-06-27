using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaaCopilotServer.Infrastructure.Migrations
{
    public partial class AddRatingCounterToOperation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DislikeCount",
                table: "CopilotOperations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "CopilotOperations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DislikeCount",
                table: "CopilotOperations");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "CopilotOperations");
        }
    }
}
