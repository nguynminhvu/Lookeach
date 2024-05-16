using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lookeach.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dollar_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DollarAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DollarAmount",
                table: "Orders");
        }
    }
}
