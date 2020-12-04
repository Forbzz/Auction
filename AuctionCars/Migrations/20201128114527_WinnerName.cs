using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionCars.Migrations
{
    public partial class WinnerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WinnerName",
                table: "CarLots",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WinnerName",
                table: "CarLots");
        }
    }
}
