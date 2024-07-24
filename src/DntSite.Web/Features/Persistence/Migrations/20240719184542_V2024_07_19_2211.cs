using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_07_19_2211 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommentsAreActive",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ShouldApproveComments",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "StackExchangeApiKey",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "UserAvatarImageOptions_AvatarsFolder",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "UserAvatarImageOptions_DefaultPhoto",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "UserAvatarImageOptions_MaxHeight",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "UserAvatarImageOptions_MaxWidth",
                table: "AppSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CommentsAreActive",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShouldApproveComments",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StackExchangeApiKey",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "UserAvatarImageOptions_AvatarsFolder",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserAvatarImageOptions_DefaultPhoto",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserAvatarImageOptions_MaxHeight",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserAvatarImageOptions_MaxWidth",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
