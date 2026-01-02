using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2025_12_31_2127 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeminiNewsFeeds_ApiKey",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GeminiNewsFeeds_NewsFeeds",
                table: "AppSettings",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeminiNewsFeeds_ApiKey",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "GeminiNewsFeeds_NewsFeeds",
                table: "AppSettings");
        }
    }
}
