using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_06_03_1205 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShouldCreatePdfsForTags",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShouldCreatePdfsForTags",
                table: "AppSettings");
        }
    }
}
