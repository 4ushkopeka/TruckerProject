using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class UserTweaks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCompanyOwner",
                table: "AspNetUsers",
                newName: "IsTruckerCompanyOwner");

            migrationBuilder.AddColumn<bool>(
                name: "IsOfferCompanyOwner",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOfferCompanyOwner",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IsTruckerCompanyOwner",
                table: "AspNetUsers",
                newName: "IsCompanyOwner");
        }
    }
}
