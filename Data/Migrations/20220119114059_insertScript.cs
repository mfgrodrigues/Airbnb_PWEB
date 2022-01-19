using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;

namespace Airbnb_PWEB.Data.Migrations
{
    public partial class insertScript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sqlContent = File.ReadAllText("./Data/Initial-Data/script.sql");
            migrationBuilder.Sql(sqlContent);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
