using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class imagesReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "PropertyImages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyImages_ReservationId",
                table: "PropertyImages",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImages_Reservations_ReservationId",
                table: "PropertyImages",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "ReservationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImages_Reservations_ReservationId",
                table: "PropertyImages");

            migrationBuilder.DropIndex(
                name: "IX_PropertyImages_ReservationId",
                table: "PropertyImages");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "PropertyImages");
        }
    }
}
