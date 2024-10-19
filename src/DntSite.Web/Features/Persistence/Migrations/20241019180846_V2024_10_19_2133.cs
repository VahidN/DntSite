using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_10_19_2133 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastVisitTime",
                table: "SiteUrls",
                newName: "LastSiteUrlVisitorStat_VisitTime");

            migrationBuilder.AddColumn<string>(
                name: "LastSiteUrlVisitorStat_DisplayName",
                table: "SiteUrls",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastSiteUrlVisitorStat_Ip",
                table: "SiteUrls",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "LastSiteUrlVisitorStat_IsSpider",
                table: "SiteUrls",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastSiteUrlVisitorStat_UserAgent",
                table: "SiteUrls",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSiteUrlVisitorStat_DisplayName",
                table: "SiteUrls");

            migrationBuilder.DropColumn(
                name: "LastSiteUrlVisitorStat_Ip",
                table: "SiteUrls");

            migrationBuilder.DropColumn(
                name: "LastSiteUrlVisitorStat_IsSpider",
                table: "SiteUrls");

            migrationBuilder.DropColumn(
                name: "LastSiteUrlVisitorStat_UserAgent",
                table: "SiteUrls");

            migrationBuilder.RenameColumn(
                name: "LastSiteUrlVisitorStat_VisitTime",
                table: "SiteUrls",
                newName: "LastVisitTime");
        }
    }
}
