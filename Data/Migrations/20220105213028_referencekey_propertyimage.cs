using Microsoft.EntityFrameworkCore.Migrations;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class referencekey_propertyimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImage_Properties_PropertyId",
                table: "PropertyImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyImage",
                table: "PropertyImage");

            migrationBuilder.RenameTable(
                name: "PropertyImage",
                newName: "PropertyImages");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyImage_PropertyId",
                table: "PropertyImages",
                newName: "IX_PropertyImages_PropertyId");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "PropertyImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyImages",
                table: "PropertyImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImages_Properties_PropertyId",
                table: "PropertyImages",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyImages_Properties_PropertyId",
                table: "PropertyImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PropertyImages",
                table: "PropertyImages");

            migrationBuilder.RenameTable(
                name: "PropertyImages",
                newName: "PropertyImage");

            migrationBuilder.RenameIndex(
                name: "IX_PropertyImages_PropertyId",
                table: "PropertyImage",
                newName: "IX_PropertyImage_PropertyId");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "PropertyImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PropertyImage",
                table: "PropertyImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyImage_Properties_PropertyId",
                table: "PropertyImage",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
