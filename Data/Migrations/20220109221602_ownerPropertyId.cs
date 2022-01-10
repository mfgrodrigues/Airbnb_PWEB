using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class ownerPropertyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Properties");
        }
    }
}
