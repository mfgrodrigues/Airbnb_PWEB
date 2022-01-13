using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class addCompanytoClientEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ClientEvaluations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientEvaluations_CompanyId",
                table: "ClientEvaluations",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientEvaluations_Companies_CompanyId",
                table: "ClientEvaluations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientEvaluations_Companies_CompanyId",
                table: "ClientEvaluations");

            migrationBuilder.DropIndex(
                name: "IX_ClientEvaluations_CompanyId",
                table: "ClientEvaluations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ClientEvaluations");
        }
    }
}
