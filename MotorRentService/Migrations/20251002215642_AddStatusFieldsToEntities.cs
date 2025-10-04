using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorRentService.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusFieldsToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Rental",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Rental",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Rental",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Motorcycles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Motorcycles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Motorcycles",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Motorcycles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Motorcycles");
        }
    }
}
