using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Apartment", "City", "Country", "Email", "Name", "Password", "Phone", "Purchases", "Role" },
                values: new object[,]
                {
                    { 1, "Av. Siempre Viva 123", "1A", "Rosario", "Argentina", "carlos@example.com", "Carlos Cliente", "client123", "3415551111", 0, "Client" },
                    { 2, "Calle Comercio 45", null, "Buenos Aires", "Argentina", "sandra@example.com", "Sandra ", "seller123", "113334444", 0, "Client" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Apartment", "City", "Country", "Email", "JoinedOn", "Name", "Password", "Phone", "Role" },
                values: new object[] { 4, "Calle Central 9", "2B", "Córdoba", "Argentina", "andres@example.com", new DateTime(2025, 8, 24, 3, 5, 42, 209, DateTimeKind.Local).AddTicks(5864), "Andrés ", "admin123", "351222333", "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Apartment", "BankAccount", "City", "Country", "Email", "Name", "Password", "Phone", "Role" },
                values: new object[] { 5, "Calle Central 9", "2B", "", "Córdoba", "Argentina", "andres@example.com", "juan ", "seller123", "351222333", "Seller" });
        }
    }
}
