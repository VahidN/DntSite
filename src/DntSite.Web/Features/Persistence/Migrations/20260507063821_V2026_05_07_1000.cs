using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_05_07_1000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaleBackupGroup_MaxZipPartSize",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaleEPubGroup_MaxZipPartSize",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TelegramBackupGroup_MaxZipPartSize",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TelegramEPubGroup_MaxZipPartSize",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaleBackupGroup_MaxZipPartSize",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "BaleEPubGroup_MaxZipPartSize",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "TelegramBackupGroup_MaxZipPartSize",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "TelegramEPubGroup_MaxZipPartSize",
                table: "AppSettings");
        }
    }
}
