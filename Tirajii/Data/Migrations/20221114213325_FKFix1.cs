using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class FKFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers");

            migrationBuilder.AddForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers");

            migrationBuilder.AddForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
