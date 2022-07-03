using Microsoft.EntityFrameworkCore.Migrations;

namespace GVAVacationManager.Migrations
{
    public partial class TeamLeadApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApprovedByTeamLead",
                table: "Leaves",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApprovedByTeamLead",
                table: "Leaves");
        }
    }
}
