using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lookeach.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class relationvoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_CategoryVouchers_CategoryVoucherId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductVouchers_ProductVoucherId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_ShopVouchers_ShopVoucherId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CategoryVouchers");

            migrationBuilder.DropTable(
                name: "ProductVouchers");

            migrationBuilder.DropTable(
                name: "ShopVouchers");

            migrationBuilder.DropIndex(
                name: "IX_Users_ShopVoucherId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductVoucherId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryVoucherId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsCategoryVoucher",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "IsProductVoucher",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "IsShopVoucher",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "ShopVoucherId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductVoucherId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryVoucherId",
                table: "Categories");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxPriceOff",
                table: "Vouchers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceCondition",
                table: "Vouchers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SlotAvailable",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SlotEachPerson",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPriceOff",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "PriceCondition",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "SlotAvailable",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "SlotEachPerson",
                table: "Vouchers");

            migrationBuilder.AddColumn<bool>(
                name: "IsCategoryVoucher",
                table: "Vouchers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProductVoucher",
                table: "Vouchers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShopVoucher",
                table: "Vouchers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ShopVoucherId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVoucherId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryVoucherId",
                table: "Categories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CategoryVouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryVouchers_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVouchers_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShopVouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopVouchers_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ShopVoucherId",
                table: "Users",
                column: "ShopVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductVoucherId",
                table: "Products",
                column: "ProductVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryVoucherId",
                table: "Categories",
                column: "CategoryVoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryVouchers_VoucherId",
                table: "CategoryVouchers",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVouchers_VoucherId",
                table: "ProductVouchers",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopVouchers_VoucherId",
                table: "ShopVouchers",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_CategoryVouchers_CategoryVoucherId",
                table: "Categories",
                column: "CategoryVoucherId",
                principalTable: "CategoryVouchers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductVouchers_ProductVoucherId",
                table: "Products",
                column: "ProductVoucherId",
                principalTable: "ProductVouchers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ShopVouchers_ShopVoucherId",
                table: "Users",
                column: "ShopVoucherId",
                principalTable: "ShopVouchers",
                principalColumn: "Id");
        }
    }
}
