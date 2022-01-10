using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class statusReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "statusReservation",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "statusReservation",
                table: "Reservations");
        }
    }
}
