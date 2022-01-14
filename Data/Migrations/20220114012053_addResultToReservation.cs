using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class addResultToReservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultEntryResultId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResultExitResultId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ResultEntryResultId",
                table: "Reservations",
                column: "ResultEntryResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ResultExitResultId",
                table: "Reservations",
                column: "ResultExitResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Results_ResultEntryResultId",
                table: "Reservations",
                column: "ResultEntryResultId",
                principalTable: "Results",
                principalColumn: "ResultId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Results_ResultExitResultId",
                table: "Reservations",
                column: "ResultExitResultId",
                principalTable: "Results",
                principalColumn: "ResultId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Results_ResultEntryResultId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Results_ResultExitResultId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ResultEntryResultId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ResultExitResultId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ResultEntryResultId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ResultExitResultId",
                table: "Reservations");
        }
    }
}
