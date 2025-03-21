using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersService.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSagaContextTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderSagaContext",
                schema: "OrderService");

            migrationBuilder.CreateTable(
                name: "OrderSagaContexts",
                schema: "OrderService",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Step = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    OrderItems = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSagaContexts", x => x.OrderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderSagaContexts",
                schema: "OrderService");

            migrationBuilder.CreateTable(
                name: "OrderSagaContext",
                schema: "OrderService",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSagaContext", x => x.OrderId);
                });
        }
    }
}
