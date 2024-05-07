using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lookeach.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class relationvouchersystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                table: "Vouchers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystem",
                table: "Vouchers");
        }
    }
}
