using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityUserService.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_usersettings_SettingsId",
                schema: "identityuserservice",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usersettings",
                schema: "identityuserservice",
                table: "usersettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                schema: "identityuserservice",
                table: "users");

            migrationBuilder.EnsureSchema(
                name: "IdentityUserService");

            migrationBuilder.RenameTable(
                name: "usersettings",
                schema: "identityuserservice",
                newName: "UserSettings",
                newSchema: "IdentityUserService");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "identityuserservice",
                newName: "Users",
                newSchema: "IdentityUserService");

            migrationBuilder.RenameIndex(
                name: "IX_users_SettingsId",
                schema: "IdentityUserService",
                table: "Users",
                newName: "IX_Users_SettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_users_Email",
                schema: "IdentityUserService",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSettings",
                schema: "IdentityUserService",
                table: "UserSettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "IdentityUserService",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserSettings_SettingsId",
                schema: "IdentityUserService",
                table: "Users",
                column: "SettingsId",
                principalSchema: "IdentityUserService",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSettings_SettingsId",
                schema: "IdentityUserService",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSettings",
                schema: "IdentityUserService",
                table: "UserSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "IdentityUserService",
                table: "Users");

            migrationBuilder.EnsureSchema(
                name: "identityuserservice");

            migrationBuilder.RenameTable(
                name: "UserSettings",
                schema: "IdentityUserService",
                newName: "usersettings",
                newSchema: "identityuserservice");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "IdentityUserService",
                newName: "users",
                newSchema: "identityuserservice");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SettingsId",
                schema: "identityuserservice",
                table: "users",
                newName: "IX_users_SettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                schema: "identityuserservice",
                table: "users",
                newName: "IX_users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_usersettings",
                schema: "identityuserservice",
                table: "usersettings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                schema: "identityuserservice",
                table: "users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_usersettings_SettingsId",
                schema: "identityuserservice",
                table: "users",
                column: "SettingsId",
                principalSchema: "identityuserservice",
                principalTable: "usersettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
