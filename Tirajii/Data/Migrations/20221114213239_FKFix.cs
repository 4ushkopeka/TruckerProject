using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class FKFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers");

            migrationBuilder.DropIndex(
                name: "IX_Truckers_UserId",
                table: "Truckers");

            migrationBuilder.CreateIndex(
                name: "IX_Truckers_UserId",
                table: "Truckers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers");

            migrationBuilder.DropIndex(
                name: "IX_Truckers_UserId",
                table: "Truckers");

            migrationBuilder.CreateIndex(
                name: "IX_Truckers_UserId",
                table: "Truckers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Truckers_AspNetUsers_UserId",
                table: "Truckers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
