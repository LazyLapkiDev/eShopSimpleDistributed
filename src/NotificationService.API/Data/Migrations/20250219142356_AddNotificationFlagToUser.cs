using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationFlagToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNotificationEnabled",
                schema: "NotificationService",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNotificationEnabled",
                schema: "NotificationService",
                table: "Users");
        }
    }
}
