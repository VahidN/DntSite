using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_07_19_2106 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SmtpServerSetting_Username",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 400);

            migrationBuilder.AlterColumn<string>(
                name: "SmtpServerSetting_Password",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 400);

            migrationBuilder.AddColumn<string>(
                name: "SmtpServerSetting_PickupFolderName",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SmtpServerSetting_ShouldValidateServerCertificate",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SmtpServerSetting_UsePickupFolder",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmtpServerSetting_PickupFolderName",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpServerSetting_ShouldValidateServerCertificate",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SmtpServerSetting_UsePickupFolder",
                table: "AppSettings");

            migrationBuilder.AlterColumn<string>(
                name: "SmtpServerSetting_Username",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 400,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SmtpServerSetting_Password",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 400,
                oldNullable: true);
        }
    }
}
