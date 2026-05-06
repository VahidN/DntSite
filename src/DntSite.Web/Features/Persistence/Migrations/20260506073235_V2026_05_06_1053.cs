using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_05_06_1053 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaleBackupGroup_AccessToken",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaleBackupGroup_ChatId",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BaleBackupGroup_IsActive",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BaleBackupGroup_ZipPassword",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaleEPubGroup_AccessToken",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaleEPubGroup_ChatId",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BaleEPubGroup_IsActive",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BaleEPubGroup_ZipPassword",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramBackupGroup_ZipPassword",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramEPubGroup_ZipPassword",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaleBackupGroup_AccessToken",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleBackupGroup_ChatId",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleBackupGroup_IsActive",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleBackupGroup_ZipPassword",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleEPubGroup_AccessToken",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleEPubGroup_ChatId",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleEPubGroup_IsActive",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleEPubGroup_ZipPassword",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "TelegramBackupGroup_ZipPassword",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "TelegramEPubGroup_ZipPassword",
                table: "AppSettings");
        }
    }
}
