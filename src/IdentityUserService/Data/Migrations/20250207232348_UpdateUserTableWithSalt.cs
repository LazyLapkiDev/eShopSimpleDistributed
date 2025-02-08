using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityUserService.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTableWithSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                schema: "identityuserservice",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                schema: "identityuserservice",
                table: "users");
        }
    }
}
