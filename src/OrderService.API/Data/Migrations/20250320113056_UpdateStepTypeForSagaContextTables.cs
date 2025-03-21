using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrdersService.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStepTypeForSagaContextTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Step", 
                table: "OrderSagaContexts", 
                schema: "OrderService");

            migrationBuilder.AddColumn<int>(name: "Step",
                schema: "OrderService",
                table: "OrderSagaContexts",
                type: "integer",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Step",
                table: "OrderSagaContexts",
                schema: "OrderService");

            migrationBuilder.AddColumn<string>(name: "Step",
                schema: "OrderService",
                table: "OrderSagaContexts",
                type: "text",
                nullable: false);
        }
    }
}
