using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class AddedTruckClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_TruckClass_ClassId",
                table: "Trucks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TruckClass",
                table: "TruckClass");

            migrationBuilder.RenameTable(
                name: "TruckClass",
                newName: "TruckClasses");

            migrationBuilder.AddColumn<string>(
                name: "Colour",
                table: "Trucks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Trucks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TruckClasses",
                table: "TruckClasses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_TruckClasses_ClassId",
                table: "Trucks",
                column: "ClassId",
                principalTable: "TruckClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_TruckClasses_ClassId",
                table: "Trucks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TruckClasses",
                table: "TruckClasses");

            migrationBuilder.DropColumn(
                name: "Colour",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Trucks");

            migrationBuilder.RenameTable(
                name: "TruckClasses",
                newName: "TruckClass");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TruckClass",
                table: "TruckClass",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_TruckClass_ClassId",
                table: "Trucks",
                column: "ClassId",
                principalTable: "TruckClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
