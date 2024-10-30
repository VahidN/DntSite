using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_10_30_1357 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SiteReferrers_DestinationUrl",
                table: "SiteReferrers");

            migrationBuilder.DropColumn(
                name: "DestinationTitle",
                table: "SiteReferrers");

            migrationBuilder.DropColumn(
                name: "DestinationUrl",
                table: "SiteReferrers");

            migrationBuilder.AddColumn<int>(
                name: "DestinationSiteUrlId",
                table: "SiteReferrers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteReferrers_DestinationSiteUrlId",
                table: "SiteReferrers",
                column: "DestinationSiteUrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_SiteReferrers_SiteUrls_DestinationSiteUrlId",
                table: "SiteReferrers",
                column: "DestinationSiteUrlId",
                principalTable: "SiteUrls",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SiteReferrers_SiteUrls_DestinationSiteUrlId",
                table: "SiteReferrers");

            migrationBuilder.DropIndex(
                name: "IX_SiteReferrers_DestinationSiteUrlId",
                table: "SiteReferrers");

            migrationBuilder.DropColumn(
                name: "DestinationSiteUrlId",
                table: "SiteReferrers");

            migrationBuilder.AddColumn<string>(
                name: "DestinationTitle",
                table: "SiteReferrers",
                type: "TEXT",
                maxLength: 1500,
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "DestinationUrl",
                table: "SiteReferrers",
                type: "TEXT",
                maxLength: 1500,
                nullable: false,
                defaultValue: "",
                collation: "NOCASE");

            migrationBuilder.CreateIndex(
                name: "IX_SiteReferrers_DestinationUrl",
                table: "SiteReferrers",
                column: "DestinationUrl");
        }
    }
}
