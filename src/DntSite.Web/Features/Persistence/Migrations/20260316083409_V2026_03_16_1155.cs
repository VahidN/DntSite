using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_03_16_1155 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeactivateSiteAfterDaysOfInactivity",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeactivateSiteAfterDaysOfInactivity",
                table: "AppSettings");
        }
    }
}
