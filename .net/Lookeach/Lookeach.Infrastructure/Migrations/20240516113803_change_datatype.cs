using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lookeach.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_datatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeCurrency",
                table: "PayPalOrders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "ExchangeCurrency",
                table: "PayPalOrders",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
