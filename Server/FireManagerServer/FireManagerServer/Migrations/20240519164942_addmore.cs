using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FireManagerServer.Migrations
{
    public partial class addmore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Modules_ModuleId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicThreshholds_Rules_RuleId",
                table: "TopicThreshholds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rules",
                table: "Rules");

            migrationBuilder.RenameTable(
                name: "Rules",
                newName: "RuleEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Rules_ModuleId",
                table: "RuleEntity",
                newName: "IX_RuleEntity_ModuleId");

            migrationBuilder.AddColumn<int>(
                name: "DeviceType",
                table: "HistoryDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RuleEntity",
                table: "RuleEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RuleEntity_Modules_ModuleId",
                table: "RuleEntity",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicThreshholds_RuleEntity_RuleId",
                table: "TopicThreshholds",
                column: "RuleId",
                principalTable: "RuleEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RuleEntity_Modules_ModuleId",
                table: "RuleEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicThreshholds_RuleEntity_RuleId",
                table: "TopicThreshholds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RuleEntity",
                table: "RuleEntity");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "HistoryDatas");

            migrationBuilder.RenameTable(
                name: "RuleEntity",
                newName: "Rules");

            migrationBuilder.RenameIndex(
                name: "IX_RuleEntity_ModuleId",
                table: "Rules",
                newName: "IX_Rules_ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rules",
                table: "Rules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Modules_ModuleId",
                table: "Rules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicThreshholds_Rules_RuleId",
                table: "TopicThreshholds",
                column: "RuleId",
                principalTable: "Rules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
