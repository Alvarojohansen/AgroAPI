using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifiSaleOrderstatusv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatusEnum",
                table: "SaleOrders");

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "SaleOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "SaleOrders");

            migrationBuilder.AddColumn<int>(
                name: "OrderStatusEnum",
                table: "SaleOrders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
