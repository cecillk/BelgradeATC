using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BelgradeATC.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHardcodedSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("07b9c6d8-d2ae-4270-f6a8-7a8b9c0d1e07"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("18cad7e9-e3bf-4381-a7b9-8b9c0d1e2f08"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("29dbe8fa-f4c0-4492-b8ca-9c0d1e2f3a09"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("3aecf90b-05d1-45a3-c9db-0d1e2f3a4b10"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("4bfd0a1c-16e2-46b4-daec-1e2f3a4b5c11"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("5c0e1b2d-27f3-47c5-ebfd-2f3a4b5c6d12"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("6d1f2c3e-3804-48d6-fc0e-3a4b5c6d7e13"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("7e203d4f-4915-49e7-0d1f-4b5c6d7e8f14"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("8f314e50-5a26-4af8-1e20-5c6d7e8f9015"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("a1f3c9d2-7b4e-4c1a-9f2e-1a2b3c4d5e01"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("b2e4d1c3-8c5f-4d2b-a1f3-2b3c4d5e6f02"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("c3d5e2f4-9d6a-4e3c-b2e4-3c4d5e6f7a03"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("d4e6f3a5-af7b-4f4d-c3d5-4d5e6f7a8b04"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("e5f7a4b6-b08c-405e-d4e6-5e6f7a8b9c05"));

            migrationBuilder.DeleteData(
                table: "ParkingSpots",
                keyColumn: "Id",
                keyValue: new Guid("f6a8b5c7-c19d-416f-e5f7-6f7a8b9c0d06"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ParkingSpots",
                columns: new[] { "Id", "OccupiedBy", "SpotNumber", "Type" },
                values: new object[,]
                {
                    { new Guid("07b9c6d8-d2ae-4270-f6a8-7a8b9c0d1e07"), null, "P2", 1 },
                    { new Guid("18cad7e9-e3bf-4381-a7b9-8b9c0d1e2f08"), null, "P3", 1 },
                    { new Guid("29dbe8fa-f4c0-4492-b8ca-9c0d1e2f3a09"), null, "P4", 1 },
                    { new Guid("3aecf90b-05d1-45a3-c9db-0d1e2f3a4b10"), null, "P5", 1 },
                    { new Guid("4bfd0a1c-16e2-46b4-daec-1e2f3a4b5c11"), null, "P6", 1 },
                    { new Guid("5c0e1b2d-27f3-47c5-ebfd-2f3a4b5c6d12"), null, "P7", 1 },
                    { new Guid("6d1f2c3e-3804-48d6-fc0e-3a4b5c6d7e13"), null, "P8", 1 },
                    { new Guid("7e203d4f-4915-49e7-0d1f-4b5c6d7e8f14"), null, "P9", 1 },
                    { new Guid("8f314e50-5a26-4af8-1e20-5c6d7e8f9015"), null, "P10", 1 },
                    { new Guid("a1f3c9d2-7b4e-4c1a-9f2e-1a2b3c4d5e01"), null, "A1", 0 },
                    { new Guid("b2e4d1c3-8c5f-4d2b-a1f3-2b3c4d5e6f02"), null, "A2", 0 },
                    { new Guid("c3d5e2f4-9d6a-4e3c-b2e4-3c4d5e6f7a03"), null, "A3", 0 },
                    { new Guid("d4e6f3a5-af7b-4f4d-c3d5-4d5e6f7a8b04"), null, "A4", 0 },
                    { new Guid("e5f7a4b6-b08c-405e-d4e6-5e6f7a8b9c05"), null, "A5", 0 },
                    { new Guid("f6a8b5c7-c19d-416f-e5f7-6f7a8b9c0d06"), null, "P1", 1 }
                });
        }
    }
}
