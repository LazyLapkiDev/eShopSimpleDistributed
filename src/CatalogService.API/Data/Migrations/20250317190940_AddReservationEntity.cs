using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogService.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                schema: "CatalogService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductReservation",
                schema: "CatalogService",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReservation", x => new { x.ProductId, x.ReservationId });
                    table.ForeignKey(
                        name: "FK_ProductReservation_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "CatalogService",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductReservation_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalSchema: "CatalogService",
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductReservation_ReservationId",
                schema: "CatalogService",
                table: "ProductReservation",
                column: "ReservationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductReservation",
                schema: "CatalogService");

            migrationBuilder.DropTable(
                name: "Reservations",
                schema: "CatalogService");
        }
    }
}
