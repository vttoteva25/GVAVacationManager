using Microsoft.EntityFrameworkCore.Migrations;

namespace GVAVacationManager.Migrations
{
    public partial class ChangeForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Leaves_UserId",
                table: "Leaves",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaves_Users_UserId",
                table: "Leaves",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaves_Users_UserId",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_UserId",
                table: "Leaves");
        }
    }
}
