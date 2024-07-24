using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_06_27_2036 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsClosed",
                table: "StackExchangeQuestions",
                newName: "IsPublic");

            migrationBuilder.RenameColumn(
                name: "BriefDescription",
                table: "StackExchangeQuestions",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "UserStat_NumberOfStackExchangeQuestionsComments",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStat_NumberOfStackExchangeQuestionsComments",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "StackExchangeQuestions",
                newName: "IsClosed");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "StackExchangeQuestions",
                newName: "BriefDescription");
        }
    }
}
