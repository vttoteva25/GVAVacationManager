using Microsoft.EntityFrameworkCore.Migrations;

namespace GVAVacationManager.Migrations
{
    public partial class ChangeLeaveModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Leaves");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Leaves",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Leaves");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Leaves",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
