using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionCars.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Cars",
                newName: "GOD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GOD",
                table: "Cars",
                newName: "Year");
        }
    }
}
