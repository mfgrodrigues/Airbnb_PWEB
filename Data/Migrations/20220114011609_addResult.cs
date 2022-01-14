using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class addResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "CheckItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckItems_ResultId",
                table: "CheckItems",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckItems_Results_ResultId",
                table: "CheckItems",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "ResultId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckItems_Results_ResultId",
                table: "CheckItems");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropIndex(
                name: "IX_CheckItems_ResultId",
                table: "CheckItems");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "CheckItems");
        }
    }
}
