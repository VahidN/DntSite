using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_07_17_1405 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinimumRequiredPosts_MaxDaysToCloseATopic",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumRequiredPosts_MinNumberOfLinksCreateANewSurvey",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumRequiredPosts_MinNumberOfLinksToCreateALearningPath",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumRequiredPosts_MinNumberOfLinksToCreateANewBacklog",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumRequiredPosts_MinPostsToCreateALearningPath",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumRequiredPosts_MinPostsToCreateANewCourse",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinimumRequiredPosts_MinPostsToCreateANewProject",
                table: "AppSettings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinimumRequiredPosts_MaxDaysToCloseATopic",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MinimumRequiredPosts_MinNumberOfLinksCreateANewSurvey",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MinimumRequiredPosts_MinNumberOfLinksToCreateALearningPath",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MinimumRequiredPosts_MinNumberOfLinksToCreateANewBacklog",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MinimumRequiredPosts_MinPostsToCreateALearningPath",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MinimumRequiredPosts_MinPostsToCreateANewCourse",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MinimumRequiredPosts_MinPostsToCreateANewProject",
                table: "AppSettings");
        }
    }
}
