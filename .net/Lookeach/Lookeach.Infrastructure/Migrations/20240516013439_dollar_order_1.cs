using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lookeach.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dollar_order_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PayPalAmounts_PaypalOrderId",
                table: "PayPalAmounts");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "PayPalItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "PayPalAmounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "ItemTotalValue",
                table: "PayPalAmounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<decimal>(
                name: "DollarPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PayPalAmounts_PaypalOrderId",
                table: "PayPalAmounts",
                column: "PaypalOrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PayPalAmounts_PaypalOrderId",
                table: "PayPalAmounts");

            migrationBuilder.DropColumn(
                name: "DollarPrice",
                table: "OrderItems");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "PayPalItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "PayPalAmounts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<double>(
                name: "ItemTotalValue",
                table: "PayPalAmounts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_PayPalAmounts_PaypalOrderId",
                table: "PayPalAmounts",
                column: "PaypalOrderId");
        }
    }
}
