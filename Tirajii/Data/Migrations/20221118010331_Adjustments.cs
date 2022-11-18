using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class Adjustments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "OwnsTruck",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Truckers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Truckers",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Truckers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OfferCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CategoryId",
                table: "Offers",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_OfferCategory_CategoryId",
                table: "Offers",
                column: "CategoryId",
                principalTable: "OfferCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_OfferCategory_CategoryId",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "OfferCategory");

            migrationBuilder.DropIndex(
                name: "IX_Offers_CategoryId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Offers");

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "Truckers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "OwnsTruck",
                table: "Truckers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
