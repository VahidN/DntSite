using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_06_18_1425 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MegaNzBackup_IsActive",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MegaNzBackup_KeepLastNFilesOnMegaNz",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MegaNzBackup_MegaBackupFolder",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MegaNzBackup_MegaEPubFolder",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MegaNzBackup_MegaEmail",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MegaNzBackup_MegaPassword",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MegaNzBackup_ZipPassword",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MegaNzBackup_IsActive",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MegaNzBackup_KeepLastNFilesOnMegaNz",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MegaNzBackup_MegaBackupFolder",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MegaNzBackup_MegaEPubFolder",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MegaNzBackup_MegaEmail",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MegaNzBackup_MegaPassword",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MegaNzBackup_ZipPassword",
                table: "AppSettings");
        }
    }
}
