using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
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
                columns: new[] { "Id", "Address", "Apartment", "City", "Country", "Email", "Name", "Password", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, "Av. Siempre Viva 123", "1A", "Rosario", "Argentina", "carlos@example.com", "Carlos Cliente", "client123", "3415551111", "Client" },
                    { 2, "Calle Comercio 45", null, "Buenos Aires", "Argentina", "sandra@example.com", "Sandra ", "seller123", "113334444", "Client" },
                    { 4, "Calle Central 9", "2B", "Córdoba", "Argentina", "andres@example.com", "Andrés ", "admin123", "351222333", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Apartment", "BankAccount", "City", "Country", "Email", "Name", "Password", "Phone", "Role" },
                values: new object[] { 5, "Calle Central 9", "2B", "", "Córdoba", "Argentina", "juan@example.com", "juan ", "seller123", "351222333", "Seller" });
        }
    }
}
