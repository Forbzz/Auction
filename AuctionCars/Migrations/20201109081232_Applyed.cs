using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionCars.Migrations
{
    public partial class Applyed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Applyed",
                table: "CarLots",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Applyed",
                table: "CarLots");
        }
    }
}
