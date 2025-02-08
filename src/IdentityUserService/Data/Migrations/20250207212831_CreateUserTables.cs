using System;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityUserService.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identityuserservice");

            migrationBuilder.CreateTable(
                name: "usersettings",
                schema: "identityuserservice",
                columns: table => new
                {
                    Id = table.Column<BigInteger>(type: "numeric", nullable: false),
                    Culture = table.Column<string>(type: "text", nullable: false),
                    IsNotificationEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "identityuserservice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SettingsId = table.Column<BigInteger>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_users_usersettings_SettingsId",
                        column: x => x.SettingsId,
                        principalSchema: "identityuserservice",
                        principalTable: "usersettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                schema: "identityuserservice",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_SettingsId",
                schema: "identityuserservice",
                table: "users",
                column: "SettingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users",
                schema: "identityuserservice");

            migrationBuilder.DropTable(
                name: "usersettings",
                schema: "identityuserservice");
        }
    }
}
