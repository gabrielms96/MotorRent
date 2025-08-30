using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotorRentService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPerson",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CNPJ = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CNHNumber = table.Column<string>(type: "text", nullable: false),
                    CNHType = table.Column<string>(type: "text", nullable: false),
                    CNHImage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPerson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motorcycles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LicensePlate = table.Column<string>(type: "text", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryPersonId = table.Column<string>(type: "text", nullable: false),
                    MotorcycleId = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Evento = table.Column<string>(type: "text", nullable: false),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rental",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DeliveryPersonId = table.Column<string>(type: "text", nullable: false),
                    MotorcycleId = table.Column<string>(type: "text", nullable: false),
                    ActualDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RealEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdPlan = table.Column<int>(type: "integer", nullable: false),
                    DailyRate = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalCost = table.Column<decimal>(type: "numeric", nullable: false),
                    FineAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    FinalValue = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RentalPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlanName = table.Column<string>(type: "text", nullable: false),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    DailyRate = table.Column<decimal>(type: "numeric", nullable: false),
                    FineRate = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalPlan", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPerson");

            migrationBuilder.DropTable(
                name: "Motorcycles");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Rental");

            migrationBuilder.DropTable(
                name: "RentalPlan");
        }
    }
}
