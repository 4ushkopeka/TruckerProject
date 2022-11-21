using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tirajii.Data.Migrations
{
    public partial class Levels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Truckers");

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "Truckers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Truckers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Truckers");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Truckers");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Truckers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
        }
    }
}
