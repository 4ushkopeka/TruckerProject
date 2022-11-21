using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class addedMappingTAbleForRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CompanyCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TruckClasses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TruckClasses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TruckClasses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TruckClasses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TruckClasses",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TruckingCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TruckingCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TruckingCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "Rates",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyRaters",
                columns: table => new
                {
                    RaterId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRaters", x => new { x.RaterId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyRaters_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyRaters_Truckers_RaterId",
                        column: x => x.RaterId,
                        principalTable: "Truckers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRaters_CompanyId",
                table: "CompanyRaters",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyRaters");

            migrationBuilder.DropColumn(
                name: "Rates",
                table: "Companies");

            migrationBuilder.InsertData(
                table: "CompanyCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "OfferProvider" },
                    { 2, "TruckProvider" }
                });

            migrationBuilder.InsertData(
                table: "TruckClasses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "BE" },
                    { 2, "C1" },
                    { 3, "C" },
                    { 4, "C1E" },
                    { 5, "CE" }
                });

            migrationBuilder.InsertData(
                table: "TruckingCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Livestock" },
                    { 2, "Food" },
                    { 3, "Items" }
                });
        }
    }
}
