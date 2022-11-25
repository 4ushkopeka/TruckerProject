using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class TruckerDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Truckers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Truckers");
        }
    }
}
