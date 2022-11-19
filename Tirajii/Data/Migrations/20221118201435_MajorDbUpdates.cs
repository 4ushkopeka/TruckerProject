using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class MajorDbUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Categories_CategoryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_OfferCategory_CategoryId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Truckers_TruckerId",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "OfferCategory");

            migrationBuilder.DropTable(
                name: "Trailers");

            migrationBuilder.DropTable(
                name: "TrailerTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TrailerId",
                table: "Trucks");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "CompanyCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Truckers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TruckerId",
                table: "Offers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompanyOwner",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrucker",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyCategories",
                table: "CompanyCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TruckingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckingCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Truckers_CategoryId",
                table: "Truckers",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_CompanyCategories_CategoryId",
                table: "Companies",
                column: "CategoryId",
                principalTable: "CompanyCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Truckers_TruckerId",
                table: "Offers",
                column: "TruckerId",
                principalTable: "Truckers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_TruckingCategories_CategoryId",
                table: "Offers",
                column: "CategoryId",
                principalTable: "TruckingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Truckers_TruckingCategories_CategoryId",
                table: "Truckers",
                column: "CategoryId",
                principalTable: "TruckingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_CompanyCategories_CategoryId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Truckers_TruckerId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_TruckingCategories_CategoryId",
                table: "Offers");

            migrationBuilder.DropForeignKey(
                name: "FK_Truckers_TruckingCategories_CategoryId",
                table: "Truckers");

            migrationBuilder.DropTable(
                name: "TruckingCategories");

            migrationBuilder.DropIndex(
                name: "IX_Truckers_CategoryId",
                table: "Truckers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyCategories",
                table: "CompanyCategories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "IsCompanyOwner",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsTrucker",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "CompanyCategories",
                newName: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "TrailerId",
                table: "Trucks",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TruckerId",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

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

            migrationBuilder.CreateTable(
                name: "TrailerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trailers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TruckId = table.Column<int>(type: "int", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasAdvertisement = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trailers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trailers_TrailerTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TrailerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trailers_Trucks_TruckId",
                        column: x => x.TruckId,
                        principalTable: "Trucks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_TruckId",
                table: "Trailers",
                column: "TruckId",
                unique: true,
                filter: "[TruckId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Trailers_TypeId",
                table: "Trailers",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Categories_CategoryId",
                table: "Companies",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_OfferCategory_CategoryId",
                table: "Offers",
                column: "CategoryId",
                principalTable: "OfferCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Truckers_TruckerId",
                table: "Offers",
                column: "TruckerId",
                principalTable: "Truckers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
