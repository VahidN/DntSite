using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_06_28_1257 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedditId",
                table: "UserSocialNetworks",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<string>(
                name: "YouTubeId",
                table: "UserSocialNetworks",
                type: "TEXT",
                maxLength: 1000,
                nullable: true,
                collation: "NOCASE");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnswer",
                table: "StackExchangeQuestionComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedditId",
                table: "UserSocialNetworks");

            migrationBuilder.DropColumn(
                name: "YouTubeId",
                table: "UserSocialNetworks");

            migrationBuilder.DropColumn(
                name: "IsAnswer",
                table: "StackExchangeQuestionComments");
        }
    }
}
