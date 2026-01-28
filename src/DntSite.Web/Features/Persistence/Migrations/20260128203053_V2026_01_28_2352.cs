using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_01_28_2352 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YouTubeDataApikey",
                table: "AppSettings",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                collation: "NOCASE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YouTubeDataApikey",
                table: "AppSettings");
        }
    }
}
