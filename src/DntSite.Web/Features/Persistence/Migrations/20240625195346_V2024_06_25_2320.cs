using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_06_25_2320 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StackExchangeQuestions_QuestionId",
                table: "StackExchangeQuestions");

            migrationBuilder.DropColumn(
                name: "NumberOfViews",
                table: "StackExchangeQuestions");

            migrationBuilder.DropColumn(
                name: "OriginalCreationDate",
                table: "StackExchangeQuestions");

            migrationBuilder.DropColumn(
                name: "OwnerUserId",
                table: "StackExchangeQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "StackExchangeQuestions");

            migrationBuilder.AlterColumn<string>(
                name: "BriefDescription",
                table: "StackExchangeQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 450,
                oldNullable: true,
                oldCollation: "NOCASE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BriefDescription",
                table: "StackExchangeQuestions",
                type: "TEXT",
                maxLength: 450,
                nullable: true,
                collation: "NOCASE",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldCollation: "NOCASE");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfViews",
                table: "StackExchangeQuestions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OriginalCreationDate",
                table: "StackExchangeQuestions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OwnerUserId",
                table: "StackExchangeQuestions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "StackExchangeQuestions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestions_QuestionId",
                table: "StackExchangeQuestions",
                column: "QuestionId",
                unique: true);
        }
    }
}
