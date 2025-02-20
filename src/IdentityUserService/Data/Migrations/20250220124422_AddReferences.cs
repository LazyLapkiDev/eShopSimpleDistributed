using System;
using System.Globalization;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityUserService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserSettings_SettingsId",
                schema: "IdentityUserService",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SettingsId",
                schema: "IdentityUserService",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                schema: "IdentityUserService",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                schema: "IdentityUserService",
                table: "UserSettings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                schema: "IdentityUserService",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSettings_Users_UserId",
                schema: "IdentityUserService",
                table: "UserSettings",
                column: "UserId",
                principalSchema: "IdentityUserService",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSettings_Users_UserId",
                schema: "IdentityUserService",
                table: "UserSettings");

            migrationBuilder.DropIndex(
                name: "IX_UserSettings_UserId",
                schema: "IdentityUserService",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "IdentityUserService",
                table: "UserSettings");

            migrationBuilder.AddColumn<BigInteger>(
                name: "SettingsId",
                schema: "IdentityUserService",
                table: "Users",
                type: "numeric",
                nullable: false,
                defaultValue: BigInteger.Parse("0", NumberFormatInfo.InvariantInfo));

            migrationBuilder.CreateIndex(
                name: "IX_Users_SettingsId",
                schema: "IdentityUserService",
                table: "Users",
                column: "SettingsId");

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
    }
}
