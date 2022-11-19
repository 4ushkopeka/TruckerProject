using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class UpdatedTrucks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBluetooth",
                table: "Trucks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCDPlayer",
                table: "Trucks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasInstaBrakes",
                table: "Trucks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasParkTronic",
                table: "Trucks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasSpeakers",
                table: "Trucks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBluetooth",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "HasCDPlayer",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "HasInstaBrakes",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "HasParkTronic",
                table: "Trucks");

            migrationBuilder.DropColumn(
                name: "HasSpeakers",
                table: "Trucks");
        }
    }
}
