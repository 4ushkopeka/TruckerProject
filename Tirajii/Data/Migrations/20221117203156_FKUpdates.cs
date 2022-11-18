using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class FKUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_Trucks_TruckId",
                table: "Trailers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Companies_CompanyId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trailers_TruckId",
                table: "Trailers");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Trucks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrailerId",
                table: "Trucks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_TruckId",
                table: "Trailers",
                column: "TruckId",
                unique: true,
                filter: "[TruckId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Trailers_Trucks_TruckId",
                table: "Trailers",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Companies_CompanyId",
                table: "Trucks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_Trucks_TruckId",
                table: "Trailers");

            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_Companies_CompanyId",
                table: "Trucks");

            migrationBuilder.DropIndex(
                name: "IX_Trailers_TruckId",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "TrailerId",
                table: "Trucks");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Trucks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_TruckId",
                table: "Trailers",
                column: "TruckId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trailers_Trucks_TruckId",
                table: "Trailers",
                column: "TruckId",
                principalTable: "Trucks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_Companies_CompanyId",
                table: "Trucks",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }
    }
}
