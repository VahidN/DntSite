using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_04_02_1410 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelegramBackupGroup_AccessToken",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TelegramBackupGroup_ChatId",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TelegramBackupGroup_IsActive",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramBackupGroup_AccessToken",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "TelegramBackupGroup_ChatId",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "TelegramBackupGroup_IsActive",
                table: "AppSettings");
        }
    }
}
