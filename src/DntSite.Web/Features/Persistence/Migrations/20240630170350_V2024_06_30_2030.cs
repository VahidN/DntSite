using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_06_30_2030 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "UserProfileComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SurveyTags");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SurveyItems");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "SurveyComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StackExchangeQuestionTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "StackExchangeQuestions");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "StackExchangeQuestionComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SearchItemTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "SearchItems");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "SearchItemComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectReleaseTags");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectReleases");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ProjectReleaseComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectIssueTypes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectIssueTags");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectIssueStatuses");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ProjectIssues");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectIssuePriorities");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ProjectIssueComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProjectFaqTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ProjectFaqs");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ProjectFaqComments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ProjectComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PrivateMessageTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "PrivateMessageComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LearningPathTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "LearningPaths");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "LearningPathComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DailyNewsItemTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "DailyNewsItems");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "DailyNewsItemComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseTopicTags");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseTopics");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseTopicComments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "CourseTopicComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseTags");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseQuestionTags");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseQuestions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseQuestionComments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "CourseQuestionComments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "CourseComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BlogPostTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "BlogPostComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "BacklogTags");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "BacklogComments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AdvertisementTags");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "AdvertisementComments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "UserProfileComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SurveyTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Surveys",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SurveyItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "SurveyComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StackExchangeQuestionTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "StackExchangeQuestions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "StackExchangeQuestionComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SearchItemTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "SearchItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "SearchItemComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectReleaseTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectReleases",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ProjectReleaseComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectIssueTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectIssueTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectIssueStatuses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ProjectIssues",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectIssuePriorities",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ProjectIssueComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProjectFaqTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ProjectFaqs",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ProjectFaqComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ProjectComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PrivateMessageTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "PrivateMessageComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LearningPathTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "LearningPaths",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "LearningPathComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DailyNewsItemTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "DailyNewsItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "DailyNewsItemComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseTopicTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseTopics",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseTopicComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "CourseTopicComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseQuestionTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseQuestions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseQuestionComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "CourseQuestionComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "CourseComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BlogPostTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "BlogPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "BlogPostComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "BacklogTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "BacklogComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AdvertisementTags",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Advertisements",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "AdvertisementComments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
