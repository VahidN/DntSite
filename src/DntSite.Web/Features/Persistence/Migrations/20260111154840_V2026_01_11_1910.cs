using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_01_11_1910 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShouldCreateNewsScreenshots",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldCreateNewsScreenshots",
                table: "AppSettings");
        }
    }
}
