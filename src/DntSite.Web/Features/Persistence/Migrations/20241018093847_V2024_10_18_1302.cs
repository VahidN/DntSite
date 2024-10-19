using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_10_18_1302 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteUrls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastVisitTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false, collation: "NOCASE"),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false, collation: "NOCASE"),
                    UrlHash = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    VisitsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsProtectedPage = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsStaticFileUrl = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteUrls_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteUrls_Url",
                table: "SiteUrls",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_SiteUrls_UrlHash",
                table: "SiteUrls",
                column: "UrlHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteUrls_UserId",
                table: "SiteUrls",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteUrls");
        }
    }
}
