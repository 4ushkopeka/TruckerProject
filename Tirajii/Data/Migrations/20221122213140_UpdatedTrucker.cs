using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class UpdatedTrucker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Truckers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PhoneNumber",
                table: "Truckers",
                type: "int",
                maxLength: 20,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Truckers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Truckers");
        }
    }
}
