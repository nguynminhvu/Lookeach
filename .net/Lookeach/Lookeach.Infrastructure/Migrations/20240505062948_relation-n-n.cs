using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lookeach.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class relationnn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Colors_ProductSourcings_ProductSourcingId",
                table: "Colors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sizes_ProductSourcings_ProductSourcingId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_ProductSourcingId",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Colors_ProductSourcingId",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "ProductSourcingId",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "ProductSourcingId",
                table: "Colors");

            migrationBuilder.CreateTable(
                name: "ColorProductSourcing",
                columns: table => new
                {
                    ColorsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductSourcingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorProductSourcing", x => new { x.ColorsId, x.ProductSourcingsId });
                    table.ForeignKey(
                        name: "FK_ColorProductSourcing_Colors_ColorsId",
                        column: x => x.ColorsId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorProductSourcing_ProductSourcings_ProductSourcingsId",
                        column: x => x.ProductSourcingsId,
                        principalTable: "ProductSourcings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSourcingSize",
                columns: table => new
                {
                    ProductSourcingsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SizesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSourcingSize", x => new { x.ProductSourcingsId, x.SizesId });
                    table.ForeignKey(
                        name: "FK_ProductSourcingSize_ProductSourcings_ProductSourcingsId",
                        column: x => x.ProductSourcingsId,
                        principalTable: "ProductSourcings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSourcingSize_Sizes_SizesId",
                        column: x => x.SizesId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorProductSourcing_ProductSourcingsId",
                table: "ColorProductSourcing",
                column: "ProductSourcingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSourcingSize_SizesId",
                table: "ProductSourcingSize",
                column: "SizesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorProductSourcing");

            migrationBuilder.DropTable(
                name: "ProductSourcingSize");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSourcingId",
                table: "Sizes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSourcingId",
                table: "Colors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_ProductSourcingId",
                table: "Sizes",
                column: "ProductSourcingId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ProductSourcingId",
                table: "Colors",
                column: "ProductSourcingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Colors_ProductSourcings_ProductSourcingId",
                table: "Colors",
                column: "ProductSourcingId",
                principalTable: "ProductSourcings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sizes_ProductSourcings_ProductSourcingId",
                table: "Sizes",
                column: "ProductSourcingId",
                principalTable: "ProductSourcings",
                principalColumn: "Id");
        }
    }
}
