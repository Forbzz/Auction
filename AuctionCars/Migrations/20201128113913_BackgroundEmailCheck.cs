using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionCars.Migrations
{
    public partial class BackgroundEmailCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ended",
                table: "CarLots",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ended",
                table: "CarLots");
        }
    }
}
