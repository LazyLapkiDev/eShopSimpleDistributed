using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameManufacturerToBrandFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_ManufacturerId",
                schema: "CatalogService",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ManufacturerId",
                schema: "CatalogService",
                table: "Products",
                newName: "BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ManufacturerId",
                schema: "CatalogService",
                table: "Products",
                newName: "IX_Products_BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                schema: "CatalogService",
                table: "Products",
                column: "BrandId",
                principalSchema: "CatalogService",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                schema: "CatalogService",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                schema: "CatalogService",
                table: "Products",
                newName: "ManufacturerId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_BrandId",
                schema: "CatalogService",
                table: "Products",
                newName: "IX_Products_ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_ManufacturerId",
                schema: "CatalogService",
                table: "Products",
                column: "ManufacturerId",
                principalSchema: "CatalogService",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
