using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireManagerServer.Migrations
{
    public partial class devicetablev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Port",
                table: "Devices",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "Devices");
        }
    }
}
