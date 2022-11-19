using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class DataInsert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Trucks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TruckClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckClass", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CompanyCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "OfferProvider" },
                    { 2, "TruckProvider" }
                });

            migrationBuilder.InsertData(
                table: "TruckClass",
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

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_ClassId",
                table: "Trucks",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trucks_TruckClass_ClassId",
                table: "Trucks",
                column: "ClassId",
                principalTable: "TruckClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trucks_TruckClass_ClassId",
                table: "Trucks");

            migrationBuilder.DropTable(
                name: "TruckClass");

            migrationBuilder.DropIndex(
                name: "IX_Trucks_ClassId",
                table: "Trucks");

            migrationBuilder.DeleteData(
                table: "CompanyCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CompanyCategories",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Trucks");
        }
    }
}
