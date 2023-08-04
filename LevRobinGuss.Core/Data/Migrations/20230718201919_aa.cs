using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevRobinGuss.Core.Data.Migrations
{
    public partial class aa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BetOnHouse",
                table: "Gamblings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BetPercentage",
                table: "Gamblings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BetOnHouse",
                table: "Gamblings");

            migrationBuilder.DropColumn(
                name: "BetPercentage",
                table: "Gamblings");
        }
    }
}
