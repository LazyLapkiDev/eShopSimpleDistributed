using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersService.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSagaContextTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "OrderService",
                table: "OrderSagaContexts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "OrderService",
                table: "OrderSagaContexts");
        }
    }
}
