using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lookeach.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class productproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSourcingImages_Images_ImageId",
                table: "ProductSourcingImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSourcingImages_ProductSourcings_ProductSourcingId",
                table: "ProductSourcingImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductSourcingImages_ImageId",
                table: "ProductSourcingImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductSourcingImages_ProductSourcingId",
                table: "ProductSourcingImages");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ProductSourcingImages");

            migrationBuilder.DropColumn(
                name: "ProductSourcingId",
                table: "ProductSourcingImages");

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "ProductSourcings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSourcingId1",
                table: "ProductSourcingImages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActived",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Sold",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSourcingImages_ProductSourcingId1",
                table: "ProductSourcingImages",
                column: "ProductSourcingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSourcingImages_ProductSourcings_ProductSourcingId1",
                table: "ProductSourcingImages",
                column: "ProductSourcingId1",
                principalTable: "ProductSourcings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSourcingImages_ProductSourcings_ProductSourcingId1",
                table: "ProductSourcingImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductSourcingImages_ProductSourcingId1",
                table: "ProductSourcingImages");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "ProductSourcings");

            migrationBuilder.DropColumn(
                name: "ProductSourcingId1",
                table: "ProductSourcingImages");

            migrationBuilder.DropColumn(
                name: "IsActived",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "ProductSourcingImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductSourcingId",
                table: "ProductSourcingImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductSourcingImages_ImageId",
                table: "ProductSourcingImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSourcingImages_ProductSourcingId",
                table: "ProductSourcingImages",
                column: "ProductSourcingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSourcingImages_Images_ImageId",
                table: "ProductSourcingImages",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSourcingImages_ProductSourcings_ProductSourcingId",
                table: "ProductSourcingImages",
                column: "ProductSourcingId",
                principalTable: "ProductSourcings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
