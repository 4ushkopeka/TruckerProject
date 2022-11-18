using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class AddedTrailerRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TruckId",
                table: "Trailers",
                type: "int",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trailers_Trucks_TruckId",
                table: "Trailers");

            migrationBuilder.DropIndex(
                name: "IX_Trailers_TruckId",
                table: "Trailers");

            migrationBuilder.DropColumn(
                name: "TruckId",
                table: "Trailers");
        }
    }
}
