using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2026_02_03_0009 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DailyNewsItemAIBacklogId",
                table: "DailyNewsItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DailyNewsItemAIBacklogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    UrlHash = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true, collation: "NOCASE"),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsProcessed = table.Column<bool>(type: "INTEGER", nullable: false),
                    FetchRetries = table.Column<int>(type: "INTEGER", nullable: false),
                    DailyNewsItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyNewsItemAIBacklogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyNewsItemAIBacklogs_DailyNewsItems_DailyNewsItemId",
                        column: x => x.DailyNewsItemId,
                        principalTable: "DailyNewsItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyNewsItemAIBacklogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemAIBacklogs_DailyNewsItemId",
                table: "DailyNewsItemAIBacklogs",
                column: "DailyNewsItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemAIBacklogs_UrlHash",
                table: "DailyNewsItemAIBacklogs",
                column: "UrlHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemAIBacklogs_UserId",
                table: "DailyNewsItemAIBacklogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyNewsItemAIBacklogs");

            migrationBuilder.DropColumn(
                name: "DailyNewsItemAIBacklogId",
                table: "DailyNewsItems");
        }
    }
}
