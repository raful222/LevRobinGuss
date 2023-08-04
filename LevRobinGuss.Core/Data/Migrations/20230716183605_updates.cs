using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LevRobinGuss.Core.Data.Migrations
{
    public partial class updates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountTokenBuy",
                table: "Users",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MetaMaskKey",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Gamblings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AmountBet = table.Column<double>(type: "float", nullable: false),
                    WinOrNot = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gamblings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gamblings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gamblings_UserId",
                table: "Gamblings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gamblings");

            migrationBuilder.DropColumn(
                name: "AmountTokenBuy",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MetaMaskKey",
                table: "Users");
        }
    }
}
