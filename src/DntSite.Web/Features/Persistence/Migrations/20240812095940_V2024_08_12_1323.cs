using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_08_12_1323 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteReferrers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastVisitTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReferrerTitle = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false, collation: "NOCASE"),
                    ReferrerUrl = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false, collation: "NOCASE"),
                    DestinationUrl = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false, collation: "NOCASE"),
                    DestinationTitle = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false, collation: "NOCASE"),
                    VisitHash = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    VisitsCount = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_SiteReferrers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteReferrers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteReferrers_UserId",
                table: "SiteReferrers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SiteReferrers_VisitHash",
                table: "SiteReferrers",
                column: "VisitHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteReferrers");
        }
    }
}
