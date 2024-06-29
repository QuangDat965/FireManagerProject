using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireManagerServer.Migrations
{
    public partial class hahah : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Apartments_ApartmentId",
                table: "Modules");

            migrationBuilder.AddColumn<string>(
                name: "InitValue",
                table: "Devices",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Apartments_ApartmentId",
                table: "Modules",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Apartments_ApartmentId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "InitValue",
                table: "Devices");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Apartments_ApartmentId",
                table: "Modules",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
