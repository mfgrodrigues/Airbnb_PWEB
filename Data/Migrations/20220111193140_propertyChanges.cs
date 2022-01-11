using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class propertyChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Properties");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Properties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_ApplicationUserId",
                table: "Properties",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_AspNetUsers_ApplicationUserId",
                table: "Properties",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_AspNetUsers_ApplicationUserId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_ApplicationUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Properties");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Properties",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
