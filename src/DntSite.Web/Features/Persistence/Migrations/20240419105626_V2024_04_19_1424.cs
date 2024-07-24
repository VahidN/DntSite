using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DntSite.Web.Features.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class V2024_04_19_1424 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FriendlyName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    HashedPassword = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    EMail = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReceiveDailyEmails = table.Column<bool>(type: "INTEGER", nullable: false),
                    EmailIsValidated = table.Column<bool>(type: "INTEGER", nullable: false),
                    RegistrationCode = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false, collation: "NOCASE"),
                    LastVisitDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsRestricted = table.Column<bool>(type: "INTEGER", nullable: false),
                    HomePageUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    Photo = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true, collation: "NOCASE"),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    IsJobsSeeker = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEmailPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfAdvertisementComments = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfAdvertisements = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfCourses = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfDrafts = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfLearningPaths = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfLinks = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfLinksComments = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfPosts = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfProjects = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfProjectsComments = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfProjectsFeedbacks = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfStackExchangeQuestions = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfSurveyComments = table.Column<int>(type: "INTEGER", nullable: false),
                    UserStat_NumberOfSurveys = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Advertisements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppDataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FriendlyName = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    XmlData = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
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
                    table.PrimaryKey("PK_AppDataProtectionKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDataProtectionKeys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppLogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    LogLevel = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Logger = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    Message = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
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
                    table.PrimaryKey("PK_AppLogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLogItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BlogName = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    SiteIsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CommentsAreActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShouldApproveComments = table.Column<bool>(type: "INTEGER", nullable: false),
                    SiteEmailsSig = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    StackExchangeApiKey = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true, collation: "NOCASE"),
                    SiteFromEmail = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    CanUsersRegister = table.Column<bool>(type: "INTEGER", nullable: false),
                    BannedUrls = table.Column<string>(type: "TEXT", nullable: false),
                    BannedSites = table.Column<string>(type: "TEXT", nullable: false),
                    BannedReferrers = table.Column<string>(type: "TEXT", nullable: false),
                    BannedEmails = table.Column<string>(type: "TEXT", nullable: false),
                    BannedPasswords = table.Column<string>(type: "TEXT", nullable: false),
                    SiteRootUri = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    SmtpServerSetting_Address = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    SmtpServerSetting_Password = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    SmtpServerSetting_Port = table.Column<int>(type: "INTEGER", nullable: false),
                    SmtpServerSetting_Username = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    UsedPasswords_ChangePasswordReminderDays = table.Column<int>(type: "INTEGER", nullable: false),
                    UsedPasswords_NotAllowedPreviouslyUsedPasswords = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAvatarImageOptions_AvatarsFolder = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    UserAvatarImageOptions_DefaultPhoto = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    UserAvatarImageOptions_MaxHeight = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAvatarImageOptions_MaxWidth = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BacklogTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BacklogTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogPostDrafts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    ReadingTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: false),
                    IsReady = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateTimeToShow = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NumberOfRequiredPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    IsConverted = table.Column<bool>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_BlogPostDrafts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostDrafts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    BriefDescription = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    EmailsSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumberOfRequiredPoints = table.Column<int>(type: "INTEGER", nullable: true),
                    OldUrl = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: true, collation: "NOCASE"),
                    ReadingTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    PingbackSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseQuestionTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuestionTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuestionTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    TopicsList = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    Requirements = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    HowToPay = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    IsFree = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumberOfPostsRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfMonthsRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfTopics = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfHelperTopics = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfQuestions = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfQuestionsComments = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    IsReadyToPublish = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumberOfTotalRatingsRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfMonthsTotalRatingsRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    Perm = table.Column<int>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CustomSidebars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_CustomSidebars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomSidebars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyNewsItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    UrlHash = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, collation: "NOCASE"),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    BriefDescription = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    PageThumbnail = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    PingbackSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastHttpStatusCode = table.Column<int>(type: "INTEGER", nullable: true),
                    LastHttpStatusCodeCheckDateTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyNewsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyNewsItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyNewsItemTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyNewsItemTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyNewsItemTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LearningPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningPaths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LearningPathTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPathTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningPathTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MassEmails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NewsTitle = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    NewsBody = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    EmailsSent = table.Column<bool>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("PK_MassEmails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MassEmails_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PrivateMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    EmailsSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    ToUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsReadByReceiver = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateMessages_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivateMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PrivateMessageTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessageTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateMessageTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectFaqTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFaqTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFaqTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssuePriorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssuePriorities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectIssuePriorities_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssueStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssueStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectIssueStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssueTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssueTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectIssueTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssueTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssueTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectIssueTypes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectReleaseTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReleaseTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReleaseTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    RequiredDependencies = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    RelatedArticles = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    DevelopersDescription = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    SourcecodeRepositoryUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    License = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    Logo = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    NumberOfFaqs = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfIssues = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfReleases = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfIssuesComments = table.Column<int>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SearchItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SearchItemTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchItemTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchItemTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StackExchangeQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    OriginalCreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnerUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    BriefDescription = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true, collation: "NOCASE"),
                    IsAnswered = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsClosed = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackExchangeQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StackExchangeQuestions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StackExchangeQuestionTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackExchangeQuestionTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StackExchangeQuestionTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Surveys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowMultipleSelection = table.Column<bool>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surveys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Surveys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SurveyTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    InUseCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyTags_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfileComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfileComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfileComments_UserProfileComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "UserProfileComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfileComments_Users_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProfileComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserSocialNetworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FacebookName = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    TwitterName = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    LinkedInProfileId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    GooglePlusProfileId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    StackOverflowId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    GithubId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    NugetId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    CodePlexId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    CodeProjectId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    SourceforgeId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    TelegramId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
                    CoffeebedeId = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true, collation: "NOCASE"),
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
                    table.PrimaryKey("PK_UserSocialNetworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSocialNetworks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserUsedPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HashedPassword = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUsedPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserUsedPasswords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertisementComments_AdvertisementComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "AdvertisementComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AdvertisementComments_Advertisements_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvertisementAdvertisementTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementAdvertisementTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_AdvertisementAdvertisementTag_AdvertisementTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "AdvertisementTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdvertisementAdvertisementTag_Advertisements_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Backlogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    DoneByUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DoneDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DaysEstimate = table.Column<int>(type: "INTEGER", nullable: true),
                    ConvertedBlogPostId = table.Column<int>(type: "INTEGER", nullable: true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backlogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backlogs_BlogPosts_ConvertedBlogPostId",
                        column: x => x.ConvertedBlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Backlogs_Users_DoneByUserId",
                        column: x => x.DoneByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Backlogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogPostComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailsSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPostComments_BlogPostComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "BlogPostComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BlogPostComments_BlogPosts_ParentId",
                        column: x => x.ParentId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BlogPostBlogPostTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostBlogPostTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_BlogPostBlogPostTag_BlogPostTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "BlogPostTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostBlogPostTag_BlogPosts_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseComments_CourseComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "CourseComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseComments_Courses_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuestions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseQuestions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DisplayId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    ReadingTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMainTopic = table.Column<bool>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopics_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseUser",
                columns: table => new
                {
                    CourseAllowedUsersId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAllowedCoursesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseUser", x => new { x.CourseAllowedUsersId, x.UserAllowedCoursesId });
                    table.ForeignKey(
                        name: "FK_CourseUser_Courses_UserAllowedCoursesId",
                        column: x => x.UserAllowedCoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseUser_Users_CourseAllowedUsersId",
                        column: x => x.CourseAllowedUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseCourseTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCourseTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CourseCourseTag_CourseTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "CourseTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseCourseTag_Courses_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyNewsItemComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailsSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyNewsItemComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyNewsItemComments_DailyNewsItemComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "DailyNewsItemComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyNewsItemComments_DailyNewsItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DailyNewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyNewsItemComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DailyNewsItemDailyNewsItemTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyNewsItemDailyNewsItemTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_DailyNewsItemDailyNewsItemTag_DailyNewsItemTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "DailyNewsItemTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyNewsItemDailyNewsItemTag_DailyNewsItems_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "DailyNewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningPathComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPathComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningPathComments_LearningPathComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "LearningPathComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LearningPathComments_LearningPaths_ParentId",
                        column: x => x.ParentId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningPathComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LearningPathLearningPathTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPathLearningPathTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_LearningPathLearningPathTag_LearningPathTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "LearningPathTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningPathLearningPathTag_LearningPaths_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivateMessageComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessageComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateMessageComments_PrivateMessageComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "PrivateMessageComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PrivateMessageComments_PrivateMessages_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PrivateMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivateMessageComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PrivateMessagePrivateMessageTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateMessagePrivateMessageTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PrivateMessagePrivateMessageTag_PrivateMessageTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "PrivateMessageTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivateMessagePrivateMessageTag_PrivateMessages_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "PrivateMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectComments_ProjectComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "ProjectComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectComments_Projects_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectFaqs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFaqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFaqs_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFaqs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    Description = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    RevisionNumber = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false, collation: "NOCASE"),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    IssuePriorityId = table.Column<int>(type: "INTEGER", nullable: false),
                    IssueTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    IssueStatusId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectIssues_ProjectIssuePriorities_IssuePriorityId",
                        column: x => x.IssuePriorityId,
                        principalTable: "ProjectIssuePriorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectIssues_ProjectIssueStatuses_IssueStatusId",
                        column: x => x.IssueStatusId,
                        principalTable: "ProjectIssueStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectIssues_ProjectIssueTypes_IssueTypeId",
                        column: x => x.IssueTypeId,
                        principalTable: "ProjectIssueTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectIssues_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectIssues_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectReleases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    FileDescription = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    NumberOfDownloads = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReleases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReleases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectReleases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectProjectTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectProjectTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProjectProjectTag_ProjectTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ProjectTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectProjectTag_Projects_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    RolesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.AssociatedEntitiesId, x.RolesId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchItemComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchItemComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchItemComments_SearchItemComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "SearchItemComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SearchItemComments_SearchItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SearchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SearchItemComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SearchItemSearchItemTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchItemSearchItemTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_SearchItemSearchItemTag_SearchItemTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "SearchItemTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SearchItemSearchItemTag_SearchItems_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "SearchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StackExchangeQuestionComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackExchangeQuestionComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StackExchangeQuestionComments_StackExchangeQuestionComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "StackExchangeQuestionComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StackExchangeQuestionComments_StackExchangeQuestions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "StackExchangeQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StackExchangeQuestionComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StackExchangeQuestionStackExchangeQuestionTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StackExchangeQuestionStackExchangeQuestionTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_StackExchangeQuestionStackExchangeQuestionTag_StackExchangeQuestionTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "StackExchangeQuestionTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StackExchangeQuestionStackExchangeQuestionTag_StackExchangeQuestions_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "StackExchangeQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailsSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyComments_SurveyComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "SurveyComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SurveyComments_Surveys_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SurveyItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    TotalSurveys = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    SurveyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyItems_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveySurveyTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveySurveyTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_SurveySurveyTag_SurveyTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "SurveyTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveySurveyTag_Surveys_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BacklogBacklogTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogBacklogTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_BacklogBacklogTag_BacklogTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "BacklogTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BacklogBacklogTag_Backlogs_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "Backlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BacklogComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BacklogComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BacklogComments_BacklogComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "BacklogComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BacklogComments_Backlogs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Backlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BacklogComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseQuestionComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuestionComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuestionComments_CourseQuestionComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "CourseQuestionComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseQuestionComments_CourseQuestions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseQuestionComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseQuestionCourseQuestionTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuestionCourseQuestionTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CourseQuestionCourseQuestionTag_CourseQuestionTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "CourseQuestionTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseQuestionCourseQuestionTag_CourseQuestions_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicComments_CourseTopicComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "CourseTopicComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseTopicComments_CourseTopics_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicCourseTopicTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicCourseTopicTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CourseTopicCourseTopicTag_CourseTopicTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "CourseTopicTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicCourseTopicTag_CourseTopics_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectFaqComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFaqComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFaqComments_ProjectFaqComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "ProjectFaqComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectFaqComments_ProjectFaqs_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProjectFaqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFaqComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectFaqProjectFaqTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFaqProjectFaqTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProjectFaqProjectFaqTag_ProjectFaqTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ProjectFaqTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectFaqProjectFaqTag_ProjectFaqs_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "ProjectFaqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssueComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmailsSent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssueComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectIssueComments_ProjectIssueComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "ProjectIssueComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectIssueComments_ProjectIssues_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProjectIssues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectIssueComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssueProjectIssueTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssueProjectIssueTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProjectIssueProjectIssueTag_ProjectIssueTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ProjectIssueTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectIssueProjectIssueTag_ProjectIssues_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "ProjectIssues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentUserFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false, collation: "NOCASE"),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    NumberOfDownloads = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 34, nullable: false, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    AdvertisementUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileUserFile_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentUserFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_Advertisements_AdvertisementUserFile_ParentId",
                        column: x => x.AdvertisementUserFile_ParentId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_Backlogs_BacklogUserFile_ParentId",
                        column: x => x.BacklogUserFile_ParentId,
                        principalTable: "Backlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_BlogPosts_BlogPostUserFile_ParentId",
                        column: x => x.BlogPostUserFile_ParentId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_CourseQuestions_CourseQuestionUserFile_ParentId",
                        column: x => x.CourseQuestionUserFile_ParentId,
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_CourseTopics_CourseTopicUserFile_ParentId",
                        column: x => x.CourseTopicUserFile_ParentId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_Courses_CourseUserFile_ParentId",
                        column: x => x.CourseUserFile_ParentId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_DailyNewsItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DailyNewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_LearningPaths_LearningPathUserFile_ParentId",
                        column: x => x.LearningPathUserFile_ParentId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_PrivateMessages_PrivateMessageUserFile_ParentId",
                        column: x => x.PrivateMessageUserFile_ParentId,
                        principalTable: "PrivateMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_ProjectFaqs_ProjectFaqUserFile_ParentId",
                        column: x => x.ProjectFaqUserFile_ParentId,
                        principalTable: "ProjectFaqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_ProjectIssues_ProjectIssueUserFile_ParentId",
                        column: x => x.ProjectIssueUserFile_ParentId,
                        principalTable: "ProjectIssues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_ProjectReleases_ProjectReleaseUserFile_ParentId",
                        column: x => x.ProjectReleaseUserFile_ParentId,
                        principalTable: "ProjectReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_Projects_ProjectUserFile_ParentId",
                        column: x => x.ProjectUserFile_ParentId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_SearchItems_SearchItemUserFile_ParentId",
                        column: x => x.SearchItemUserFile_ParentId,
                        principalTable: "SearchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_StackExchangeQuestions_StackExchangeQuestionUserFile_ParentId",
                        column: x => x.StackExchangeQuestionUserFile_ParentId,
                        principalTable: "StackExchangeQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_Surveys_SurveyUserFile_ParentId",
                        column: x => x.SurveyUserFile_ParentId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentUserFiles_Users_UserProfileUserFile_ParentId",
                        column: x => x.UserProfileUserFile_ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectReleaseComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EntityStat_NumberOfBookmarks = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfComments = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfReactions = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfTags = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViews = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityStat_NumberOfViewsFromFeed = table.Column<int>(type: "INTEGER", nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Rating_AverageRating = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rating_TotalRaters = table.Column<int>(type: "INTEGER", nullable: false),
                    Rating_TotalRating = table.Column<long>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ReplyId = table.Column<int>(type: "INTEGER", nullable: true),
                    Body = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    AuditActions = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReleaseComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectReleaseComments_ProjectReleaseComments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "ProjectReleaseComments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjectReleaseComments_ProjectReleases_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProjectReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectReleaseComments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjectReleaseProjectReleaseTag",
                columns: table => new
                {
                    AssociatedEntitiesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectReleaseProjectReleaseTag", x => new { x.AssociatedEntitiesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProjectReleaseProjectReleaseTag_ProjectReleaseTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ProjectReleaseTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectReleaseProjectReleaseTag_ProjectReleases_AssociatedEntitiesId",
                        column: x => x.AssociatedEntitiesId,
                        principalTable: "ProjectReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyItemUser",
                columns: table => new
                {
                    SurveyItemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyItemUser", x => new { x.SurveyItemsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_SurveyItemUser_SurveyItems_SurveyItemsId",
                        column: x => x.SurveyItemsId,
                        principalTable: "SurveyItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyItemUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentBookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 55, nullable: false, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    AdvertisementBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    AdvertisementCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    DailyNewsItemBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileCommentBookmark_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentBookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_AdvertisementComments_AdvertisementCommentBookmark_ParentId",
                        column: x => x.AdvertisementCommentBookmark_ParentId,
                        principalTable: "AdvertisementComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_Advertisements_AdvertisementBookmark_ParentId",
                        column: x => x.AdvertisementBookmark_ParentId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_BacklogComments_BacklogCommentBookmark_ParentId",
                        column: x => x.BacklogCommentBookmark_ParentId,
                        principalTable: "BacklogComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_Backlogs_BacklogBookmark_ParentId",
                        column: x => x.BacklogBookmark_ParentId,
                        principalTable: "Backlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_BlogPostComments_BlogPostCommentBookmark_ParentId",
                        column: x => x.BlogPostCommentBookmark_ParentId,
                        principalTable: "BlogPostComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_BlogPosts_BlogPostBookmark_ParentId",
                        column: x => x.BlogPostBookmark_ParentId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_CourseComments_CourseCommentBookmark_ParentId",
                        column: x => x.CourseCommentBookmark_ParentId,
                        principalTable: "CourseComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_CourseQuestionComments_CourseQuestionCommentBookmark_ParentId",
                        column: x => x.CourseQuestionCommentBookmark_ParentId,
                        principalTable: "CourseQuestionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_CourseQuestions_CourseQuestionBookmark_ParentId",
                        column: x => x.CourseQuestionBookmark_ParentId,
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_CourseTopicComments_CourseTopicCommentBookmark_ParentId",
                        column: x => x.CourseTopicCommentBookmark_ParentId,
                        principalTable: "CourseTopicComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_CourseTopics_CourseTopicBookmark_ParentId",
                        column: x => x.CourseTopicBookmark_ParentId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_Courses_CourseBookmark_ParentId",
                        column: x => x.CourseBookmark_ParentId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_DailyNewsItemComments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DailyNewsItemComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_DailyNewsItems_DailyNewsItemBookmark_ParentId",
                        column: x => x.DailyNewsItemBookmark_ParentId,
                        principalTable: "DailyNewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_LearningPathComments_LearningPathCommentBookmark_ParentId",
                        column: x => x.LearningPathCommentBookmark_ParentId,
                        principalTable: "LearningPathComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_LearningPaths_LearningPathBookmark_ParentId",
                        column: x => x.LearningPathBookmark_ParentId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_PrivateMessageComments_PrivateMessageCommentBookmark_ParentId",
                        column: x => x.PrivateMessageCommentBookmark_ParentId,
                        principalTable: "PrivateMessageComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_PrivateMessages_PrivateMessageBookmark_ParentId",
                        column: x => x.PrivateMessageBookmark_ParentId,
                        principalTable: "PrivateMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_ProjectComments_ProjectCommentBookmark_ParentId",
                        column: x => x.ProjectCommentBookmark_ParentId,
                        principalTable: "ProjectComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_ProjectFaqComments_ProjectFaqCommentBookmark_ParentId",
                        column: x => x.ProjectFaqCommentBookmark_ParentId,
                        principalTable: "ProjectFaqComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_ProjectFaqs_ProjectFaqBookmark_ParentId",
                        column: x => x.ProjectFaqBookmark_ParentId,
                        principalTable: "ProjectFaqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_ProjectIssueComments_ProjectIssueCommentBookmark_ParentId",
                        column: x => x.ProjectIssueCommentBookmark_ParentId,
                        principalTable: "ProjectIssueComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_ProjectIssues_ProjectIssueBookmark_ParentId",
                        column: x => x.ProjectIssueBookmark_ParentId,
                        principalTable: "ProjectIssues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_ProjectReleaseComments_ProjectReleaseCommentBookmark_ParentId",
                        column: x => x.ProjectReleaseCommentBookmark_ParentId,
                        principalTable: "ProjectReleaseComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_ProjectReleases_ProjectReleaseBookmark_ParentId",
                        column: x => x.ProjectReleaseBookmark_ParentId,
                        principalTable: "ProjectReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_Projects_ProjectBookmark_ParentId",
                        column: x => x.ProjectBookmark_ParentId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_SearchItemComments_SearchItemCommentBookmark_ParentId",
                        column: x => x.SearchItemCommentBookmark_ParentId,
                        principalTable: "SearchItemComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_SearchItems_SearchItemBookmark_ParentId",
                        column: x => x.SearchItemBookmark_ParentId,
                        principalTable: "SearchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_StackExchangeQuestionComments_StackExchangeQuestionCommentBookmark_ParentId",
                        column: x => x.StackExchangeQuestionCommentBookmark_ParentId,
                        principalTable: "StackExchangeQuestionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_StackExchangeQuestions_StackExchangeQuestionBookmark_ParentId",
                        column: x => x.StackExchangeQuestionBookmark_ParentId,
                        principalTable: "StackExchangeQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_SurveyComments_SurveyCommentBookmark_ParentId",
                        column: x => x.SurveyCommentBookmark_ParentId,
                        principalTable: "SurveyComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_Surveys_SurveyBookmark_ParentId",
                        column: x => x.SurveyBookmark_ParentId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_UserProfileComments_UserProfileCommentBookmark_ParentId",
                        column: x => x.UserProfileCommentBookmark_ParentId,
                        principalTable: "UserProfileComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentBookmarks_Users_UserProfileBookmark_ParentId",
                        column: x => x.UserProfileBookmark_ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentReactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Reaction = table.Column<int>(type: "INTEGER", nullable: false),
                    ForUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 55, nullable: false, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    AdvertisementCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    AdvertisementReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    DailyNewsItemCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileCommentReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileReaction_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentReactions_AdvertisementComments_AdvertisementCommentReaction_ParentId",
                        column: x => x.AdvertisementCommentReaction_ParentId,
                        principalTable: "AdvertisementComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_Advertisements_AdvertisementReaction_ParentId",
                        column: x => x.AdvertisementReaction_ParentId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_BacklogComments_BacklogCommentReaction_ParentId",
                        column: x => x.BacklogCommentReaction_ParentId,
                        principalTable: "BacklogComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_Backlogs_BacklogReaction_ParentId",
                        column: x => x.BacklogReaction_ParentId,
                        principalTable: "Backlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_BlogPostComments_BlogPostCommentReaction_ParentId",
                        column: x => x.BlogPostCommentReaction_ParentId,
                        principalTable: "BlogPostComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_BlogPosts_BlogPostReaction_ParentId",
                        column: x => x.BlogPostReaction_ParentId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_CourseComments_CourseCommentReaction_ParentId",
                        column: x => x.CourseCommentReaction_ParentId,
                        principalTable: "CourseComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_CourseQuestionComments_CourseQuestionCommentReaction_ParentId",
                        column: x => x.CourseQuestionCommentReaction_ParentId,
                        principalTable: "CourseQuestionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_CourseQuestions_CourseQuestionReaction_ParentId",
                        column: x => x.CourseQuestionReaction_ParentId,
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_CourseTopicComments_CourseTopicCommentReaction_ParentId",
                        column: x => x.CourseTopicCommentReaction_ParentId,
                        principalTable: "CourseTopicComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_CourseTopics_CourseTopicReaction_ParentId",
                        column: x => x.CourseTopicReaction_ParentId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_Courses_CourseReaction_ParentId",
                        column: x => x.CourseReaction_ParentId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_DailyNewsItemComments_DailyNewsItemCommentReaction_ParentId",
                        column: x => x.DailyNewsItemCommentReaction_ParentId,
                        principalTable: "DailyNewsItemComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_DailyNewsItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DailyNewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_LearningPathComments_LearningPathCommentReaction_ParentId",
                        column: x => x.LearningPathCommentReaction_ParentId,
                        principalTable: "LearningPathComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_LearningPaths_LearningPathReaction_ParentId",
                        column: x => x.LearningPathReaction_ParentId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_PrivateMessageComments_PrivateMessageCommentReaction_ParentId",
                        column: x => x.PrivateMessageCommentReaction_ParentId,
                        principalTable: "PrivateMessageComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_PrivateMessages_PrivateMessageReaction_ParentId",
                        column: x => x.PrivateMessageReaction_ParentId,
                        principalTable: "PrivateMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_ProjectComments_ProjectCommentReaction_ParentId",
                        column: x => x.ProjectCommentReaction_ParentId,
                        principalTable: "ProjectComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_ProjectFaqComments_ProjectFaqCommentReaction_ParentId",
                        column: x => x.ProjectFaqCommentReaction_ParentId,
                        principalTable: "ProjectFaqComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_ProjectFaqs_ProjectFaqReaction_ParentId",
                        column: x => x.ProjectFaqReaction_ParentId,
                        principalTable: "ProjectFaqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_ProjectIssueComments_ProjectIssueCommentReaction_ParentId",
                        column: x => x.ProjectIssueCommentReaction_ParentId,
                        principalTable: "ProjectIssueComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_ProjectIssues_ProjectIssueReaction_ParentId",
                        column: x => x.ProjectIssueReaction_ParentId,
                        principalTable: "ProjectIssues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_ProjectReleaseComments_ProjectReleaseCommentReaction_ParentId",
                        column: x => x.ProjectReleaseCommentReaction_ParentId,
                        principalTable: "ProjectReleaseComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_ProjectReleases_ProjectReleaseReaction_ParentId",
                        column: x => x.ProjectReleaseReaction_ParentId,
                        principalTable: "ProjectReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_Projects_ProjectReaction_ParentId",
                        column: x => x.ProjectReaction_ParentId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_SearchItemComments_SearchItemCommentReaction_ParentId",
                        column: x => x.SearchItemCommentReaction_ParentId,
                        principalTable: "SearchItemComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_SearchItems_SearchItemReaction_ParentId",
                        column: x => x.SearchItemReaction_ParentId,
                        principalTable: "SearchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_StackExchangeQuestionComments_StackExchangeQuestionCommentReaction_ParentId",
                        column: x => x.StackExchangeQuestionCommentReaction_ParentId,
                        principalTable: "StackExchangeQuestionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_StackExchangeQuestions_StackExchangeQuestionReaction_ParentId",
                        column: x => x.StackExchangeQuestionReaction_ParentId,
                        principalTable: "StackExchangeQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_SurveyComments_SurveyCommentReaction_ParentId",
                        column: x => x.SurveyCommentReaction_ParentId,
                        principalTable: "SurveyComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_Surveys_SurveyReaction_ParentId",
                        column: x => x.SurveyReaction_ParentId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_UserProfileComments_UserProfileCommentReaction_ParentId",
                        column: x => x.UserProfileCommentReaction_ParentId,
                        principalTable: "UserProfileComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentReactions_Users_ForUserId",
                        column: x => x.ForUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentReactions_Users_UserProfileReaction_ParentId",
                        column: x => x.UserProfileReaction_ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentVisitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsFromFeed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 55, nullable: false, collation: "NOCASE"),
                    Audit_CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Audit_CreatedAtPersian = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Audit_CreatedByUserAgent = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Audit_CreatedByUserIp = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Country_Code = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Country_Name = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Device_Brand = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Device_Family = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Device_IsSpider = table.Column<bool>(type: "INTEGER", nullable: false),
                    Device_Model = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    GuestUser_HomeUrl = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    GuestUser_UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Os_Family = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Os_Major = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Os_Minor = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Os_Patch = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Os_PatchMinor = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Referrer_OriginalTitle = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: true),
                    Referrer_OriginalUrl = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: true),
                    Referrer_Title = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: true),
                    Referrer_Url = table.Column<string>(type: "TEXT", maxLength: 1500, nullable: true),
                    UserAgent_Family = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    UserAgent_Major = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    UserAgent_Minor = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    UserAgent_Patch = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    AdvertisementCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    AdvertisementUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    AdvertisementVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BacklogVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseQuestionVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTopicVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    DailyNewsItemCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    DailyNewsItemUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    BlogPostVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    PrivateMessageVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectFaqVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectIssueVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectReleaseVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    LearningPathVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SearchItemVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    StackExchangeQuestionVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SurveyVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileCommentVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileUserFileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserProfileVisitor_ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentVisitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_AdvertisementComments_AdvertisementCommentVisitor_ParentId",
                        column: x => x.AdvertisementCommentVisitor_ParentId,
                        principalTable: "AdvertisementComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_Advertisements_AdvertisementVisitor_ParentId",
                        column: x => x.AdvertisementVisitor_ParentId,
                        principalTable: "Advertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_BacklogComments_BacklogCommentVisitor_ParentId",
                        column: x => x.BacklogCommentVisitor_ParentId,
                        principalTable: "BacklogComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_Backlogs_BacklogVisitor_ParentId",
                        column: x => x.BacklogVisitor_ParentId,
                        principalTable: "Backlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_BlogPostComments_BlogPostCommentVisitor_ParentId",
                        column: x => x.BlogPostCommentVisitor_ParentId,
                        principalTable: "BlogPostComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_BlogPosts_BlogPostVisitor_ParentId",
                        column: x => x.BlogPostVisitor_ParentId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_CourseComments_CourseCommentVisitor_ParentId",
                        column: x => x.CourseCommentVisitor_ParentId,
                        principalTable: "CourseComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_CourseQuestionComments_CourseQuestionCommentVisitor_ParentId",
                        column: x => x.CourseQuestionCommentVisitor_ParentId,
                        principalTable: "CourseQuestionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_CourseQuestions_CourseQuestionVisitor_ParentId",
                        column: x => x.CourseQuestionVisitor_ParentId,
                        principalTable: "CourseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_CourseTopicComments_CourseTopicCommentVisitor_ParentId",
                        column: x => x.CourseTopicCommentVisitor_ParentId,
                        principalTable: "CourseTopicComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_CourseTopics_CourseTopicVisitor_ParentId",
                        column: x => x.CourseTopicVisitor_ParentId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_Courses_CourseVisitor_ParentId",
                        column: x => x.CourseVisitor_ParentId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_DailyNewsItemComments_DailyNewsItemCommentVisitor_ParentId",
                        column: x => x.DailyNewsItemCommentVisitor_ParentId,
                        principalTable: "DailyNewsItemComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_DailyNewsItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DailyNewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_LearningPathComments_LearningPathCommentVisitor_ParentId",
                        column: x => x.LearningPathCommentVisitor_ParentId,
                        principalTable: "LearningPathComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_LearningPaths_LearningPathVisitor_ParentId",
                        column: x => x.LearningPathVisitor_ParentId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_AdvertisementUserFileVisitor_ParentId",
                        column: x => x.AdvertisementUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_BacklogUserFileVisitor_ParentId",
                        column: x => x.BacklogUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_BlogPostUserFileVisitor_ParentId",
                        column: x => x.BlogPostUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_CourseQuestionUserFileVisitor_ParentId",
                        column: x => x.CourseQuestionUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_CourseTopicUserFileVisitor_ParentId",
                        column: x => x.CourseTopicUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_CourseUserFileVisitor_ParentId",
                        column: x => x.CourseUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_DailyNewsItemUserFileVisitor_ParentId",
                        column: x => x.DailyNewsItemUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_LearningPathUserFileVisitor_ParentId",
                        column: x => x.LearningPathUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_PrivateMessageUserFileVisitor_ParentId",
                        column: x => x.PrivateMessageUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_ProjectFaqUserFileVisitor_ParentId",
                        column: x => x.ProjectFaqUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_ProjectIssueUserFileVisitor_ParentId",
                        column: x => x.ProjectIssueUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_ProjectReleaseUserFileVisitor_ParentId",
                        column: x => x.ProjectReleaseUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_ProjectUserFileVisitor_ParentId",
                        column: x => x.ProjectUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_SearchItemUserFileVisitor_ParentId",
                        column: x => x.SearchItemUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_StackExchangeQuestionUserFileVisitor_ParentId",
                        column: x => x.StackExchangeQuestionUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_SurveyUserFileVisitor_ParentId",
                        column: x => x.SurveyUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ParentUserFiles_UserProfileUserFileVisitor_ParentId",
                        column: x => x.UserProfileUserFileVisitor_ParentId,
                        principalTable: "ParentUserFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_PrivateMessageComments_PrivateMessageCommentVisitor_ParentId",
                        column: x => x.PrivateMessageCommentVisitor_ParentId,
                        principalTable: "PrivateMessageComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_PrivateMessages_PrivateMessageVisitor_ParentId",
                        column: x => x.PrivateMessageVisitor_ParentId,
                        principalTable: "PrivateMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ProjectComments_ProjectCommentVisitor_ParentId",
                        column: x => x.ProjectCommentVisitor_ParentId,
                        principalTable: "ProjectComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ProjectFaqComments_ProjectFaqCommentVisitor_ParentId",
                        column: x => x.ProjectFaqCommentVisitor_ParentId,
                        principalTable: "ProjectFaqComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ProjectFaqs_ProjectFaqVisitor_ParentId",
                        column: x => x.ProjectFaqVisitor_ParentId,
                        principalTable: "ProjectFaqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ProjectIssueComments_ProjectIssueCommentVisitor_ParentId",
                        column: x => x.ProjectIssueCommentVisitor_ParentId,
                        principalTable: "ProjectIssueComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ProjectIssues_ProjectIssueVisitor_ParentId",
                        column: x => x.ProjectIssueVisitor_ParentId,
                        principalTable: "ProjectIssues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ProjectReleaseComments_ProjectReleaseCommentVisitor_ParentId",
                        column: x => x.ProjectReleaseCommentVisitor_ParentId,
                        principalTable: "ProjectReleaseComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_ProjectReleases_ProjectReleaseVisitor_ParentId",
                        column: x => x.ProjectReleaseVisitor_ParentId,
                        principalTable: "ProjectReleases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_Projects_ProjectVisitor_ParentId",
                        column: x => x.ProjectVisitor_ParentId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_SearchItemComments_SearchItemCommentVisitor_ParentId",
                        column: x => x.SearchItemCommentVisitor_ParentId,
                        principalTable: "SearchItemComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_SearchItems_SearchItemVisitor_ParentId",
                        column: x => x.SearchItemVisitor_ParentId,
                        principalTable: "SearchItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_StackExchangeQuestionComments_StackExchangeQuestionCommentVisitor_ParentId",
                        column: x => x.StackExchangeQuestionCommentVisitor_ParentId,
                        principalTable: "StackExchangeQuestionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_StackExchangeQuestions_StackExchangeQuestionVisitor_ParentId",
                        column: x => x.StackExchangeQuestionVisitor_ParentId,
                        principalTable: "StackExchangeQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_SurveyComments_SurveyCommentVisitor_ParentId",
                        column: x => x.SurveyCommentVisitor_ParentId,
                        principalTable: "SurveyComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_Surveys_SurveyVisitor_ParentId",
                        column: x => x.SurveyVisitor_ParentId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_UserProfileComments_UserProfileCommentVisitor_ParentId",
                        column: x => x.UserProfileCommentVisitor_ParentId,
                        principalTable: "UserProfileComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentVisitors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ParentVisitors_Users_UserProfileVisitor_ParentId",
                        column: x => x.UserProfileVisitor_ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementAdvertisementTag_TagsId",
                table: "AdvertisementAdvertisementTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementComments_ParentId",
                table: "AdvertisementComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementComments_ReplyId",
                table: "AdvertisementComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementComments_UserId",
                table: "AdvertisementComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisements_UserId",
                table: "Advertisements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementTags_Name",
                table: "AdvertisementTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementTags_UserId",
                table: "AdvertisementTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDataProtectionKeys_UserId",
                table: "AppDataProtectionKeys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLogItems_UserId",
                table: "AppLogItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettings_UserId",
                table: "AppSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogBacklogTag_TagsId",
                table: "BacklogBacklogTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogComments_ParentId",
                table: "BacklogComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogComments_ReplyId",
                table: "BacklogComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogComments_UserId",
                table: "BacklogComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Backlogs_ConvertedBlogPostId",
                table: "Backlogs",
                column: "ConvertedBlogPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Backlogs_DoneByUserId",
                table: "Backlogs",
                column: "DoneByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Backlogs_UserId",
                table: "Backlogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BacklogTags_Name",
                table: "BacklogTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BacklogTags_UserId",
                table: "BacklogTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostBlogPostTag_TagsId",
                table: "BlogPostBlogPostTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_ParentId",
                table: "BlogPostComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_ReplyId",
                table: "BlogPostComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComments_UserId",
                table: "BlogPostComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostDrafts_UserId",
                table: "BlogPostDrafts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_UserId",
                table: "BlogPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTags_Name",
                table: "BlogPostTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTags_UserId",
                table: "BlogPostTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_ParentId",
                table: "CourseComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_ReplyId",
                table: "CourseComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_UserId",
                table: "CourseComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseCourseTag_TagsId",
                table: "CourseCourseTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionComments_ParentId",
                table: "CourseQuestionComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionComments_ReplyId",
                table: "CourseQuestionComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionComments_UserId",
                table: "CourseQuestionComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionCourseQuestionTag_TagsId",
                table: "CourseQuestionCourseQuestionTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestions_CourseId",
                table: "CourseQuestions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestions_UserId",
                table: "CourseQuestions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionTags_Name",
                table: "CourseQuestionTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuestionTags_UserId",
                table: "CourseQuestionTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_UserId",
                table: "Courses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTags_Name",
                table: "CourseTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTags_UserId",
                table: "CourseTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicComments_ParentId",
                table: "CourseTopicComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicComments_ReplyId",
                table: "CourseTopicComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicComments_UserId",
                table: "CourseTopicComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicCourseTopicTag_TagsId",
                table: "CourseTopicCourseTopicTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopics_CourseId",
                table: "CourseTopics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopics_UserId",
                table: "CourseTopics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicTags_Name",
                table: "CourseTopicTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicTags_UserId",
                table: "CourseTopicTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseUser_UserAllowedCoursesId",
                table: "CourseUser",
                column: "UserAllowedCoursesId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomSidebars_UserId",
                table: "CustomSidebars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemComments_ParentId",
                table: "DailyNewsItemComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemComments_ReplyId",
                table: "DailyNewsItemComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemComments_UserId",
                table: "DailyNewsItemComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemDailyNewsItemTag_TagsId",
                table: "DailyNewsItemDailyNewsItemTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItems_UrlHash",
                table: "DailyNewsItems",
                column: "UrlHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItems_UserId",
                table: "DailyNewsItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemTags_Name",
                table: "DailyNewsItemTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyNewsItemTags_UserId",
                table: "DailyNewsItemTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathComments_ParentId",
                table: "LearningPathComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathComments_ReplyId",
                table: "LearningPathComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathComments_UserId",
                table: "LearningPathComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathLearningPathTag_TagsId",
                table: "LearningPathLearningPathTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPaths_UserId",
                table: "LearningPaths",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathTags_Name",
                table: "LearningPathTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathTags_UserId",
                table: "LearningPathTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MassEmails_UserId",
                table: "MassEmails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_AdvertisementBookmark_ParentId",
                table: "ParentBookmarks",
                column: "AdvertisementBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_AdvertisementCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "AdvertisementCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_BacklogBookmark_ParentId",
                table: "ParentBookmarks",
                column: "BacklogBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_BacklogCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "BacklogCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_BlogPostBookmark_ParentId",
                table: "ParentBookmarks",
                column: "BlogPostBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_BlogPostCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "BlogPostCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_CourseBookmark_ParentId",
                table: "ParentBookmarks",
                column: "CourseBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_CourseCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "CourseCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_CourseQuestionBookmark_ParentId",
                table: "ParentBookmarks",
                column: "CourseQuestionBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_CourseQuestionCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "CourseQuestionCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_CourseTopicBookmark_ParentId",
                table: "ParentBookmarks",
                column: "CourseTopicBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_CourseTopicCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "CourseTopicCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_DailyNewsItemBookmark_ParentId",
                table: "ParentBookmarks",
                column: "DailyNewsItemBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_LearningPathBookmark_ParentId",
                table: "ParentBookmarks",
                column: "LearningPathBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_LearningPathCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "LearningPathCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ParentId",
                table: "ParentBookmarks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_PrivateMessageBookmark_ParentId",
                table: "ParentBookmarks",
                column: "PrivateMessageBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_PrivateMessageCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "PrivateMessageCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectFaqBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectFaqBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectFaqCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectFaqCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectIssueBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectIssueBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectIssueCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectIssueCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectReleaseBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectReleaseBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_ProjectReleaseCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "ProjectReleaseCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_SearchItemBookmark_ParentId",
                table: "ParentBookmarks",
                column: "SearchItemBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_SearchItemCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "SearchItemCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_StackExchangeQuestionBookmark_ParentId",
                table: "ParentBookmarks",
                column: "StackExchangeQuestionBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_StackExchangeQuestionCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "StackExchangeQuestionCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_SurveyBookmark_ParentId",
                table: "ParentBookmarks",
                column: "SurveyBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_SurveyCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "SurveyCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_UserId",
                table: "ParentBookmarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_UserProfileBookmark_ParentId",
                table: "ParentBookmarks",
                column: "UserProfileBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentBookmarks_UserProfileCommentBookmark_ParentId",
                table: "ParentBookmarks",
                column: "UserProfileCommentBookmark_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_AdvertisementCommentReaction_ParentId",
                table: "ParentReactions",
                column: "AdvertisementCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_AdvertisementReaction_ParentId",
                table: "ParentReactions",
                column: "AdvertisementReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_BacklogCommentReaction_ParentId",
                table: "ParentReactions",
                column: "BacklogCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_BacklogReaction_ParentId",
                table: "ParentReactions",
                column: "BacklogReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_BlogPostCommentReaction_ParentId",
                table: "ParentReactions",
                column: "BlogPostCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_BlogPostReaction_ParentId",
                table: "ParentReactions",
                column: "BlogPostReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_CourseCommentReaction_ParentId",
                table: "ParentReactions",
                column: "CourseCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_CourseQuestionCommentReaction_ParentId",
                table: "ParentReactions",
                column: "CourseQuestionCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_CourseQuestionReaction_ParentId",
                table: "ParentReactions",
                column: "CourseQuestionReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_CourseReaction_ParentId",
                table: "ParentReactions",
                column: "CourseReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_CourseTopicCommentReaction_ParentId",
                table: "ParentReactions",
                column: "CourseTopicCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_CourseTopicReaction_ParentId",
                table: "ParentReactions",
                column: "CourseTopicReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_DailyNewsItemCommentReaction_ParentId",
                table: "ParentReactions",
                column: "DailyNewsItemCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ForUserId",
                table: "ParentReactions",
                column: "ForUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_LearningPathCommentReaction_ParentId",
                table: "ParentReactions",
                column: "LearningPathCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_LearningPathReaction_ParentId",
                table: "ParentReactions",
                column: "LearningPathReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ParentId",
                table: "ParentReactions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_PrivateMessageCommentReaction_ParentId",
                table: "ParentReactions",
                column: "PrivateMessageCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_PrivateMessageReaction_ParentId",
                table: "ParentReactions",
                column: "PrivateMessageReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectCommentReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectFaqCommentReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectFaqCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectFaqReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectFaqReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectIssueCommentReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectIssueCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectIssueReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectIssueReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectReleaseCommentReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectReleaseCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_ProjectReleaseReaction_ParentId",
                table: "ParentReactions",
                column: "ProjectReleaseReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_SearchItemCommentReaction_ParentId",
                table: "ParentReactions",
                column: "SearchItemCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_SearchItemReaction_ParentId",
                table: "ParentReactions",
                column: "SearchItemReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_StackExchangeQuestionCommentReaction_ParentId",
                table: "ParentReactions",
                column: "StackExchangeQuestionCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_StackExchangeQuestionReaction_ParentId",
                table: "ParentReactions",
                column: "StackExchangeQuestionReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_SurveyCommentReaction_ParentId",
                table: "ParentReactions",
                column: "SurveyCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_SurveyReaction_ParentId",
                table: "ParentReactions",
                column: "SurveyReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_UserId",
                table: "ParentReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_UserProfileCommentReaction_ParentId",
                table: "ParentReactions",
                column: "UserProfileCommentReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentReactions_UserProfileReaction_ParentId",
                table: "ParentReactions",
                column: "UserProfileReaction_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_AdvertisementUserFile_ParentId",
                table: "ParentUserFiles",
                column: "AdvertisementUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_BacklogUserFile_ParentId",
                table: "ParentUserFiles",
                column: "BacklogUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_BlogPostUserFile_ParentId",
                table: "ParentUserFiles",
                column: "BlogPostUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_CourseQuestionUserFile_ParentId",
                table: "ParentUserFiles",
                column: "CourseQuestionUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_CourseTopicUserFile_ParentId",
                table: "ParentUserFiles",
                column: "CourseTopicUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_CourseUserFile_ParentId",
                table: "ParentUserFiles",
                column: "CourseUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_LearningPathUserFile_ParentId",
                table: "ParentUserFiles",
                column: "LearningPathUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_ParentId",
                table: "ParentUserFiles",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_PrivateMessageUserFile_ParentId",
                table: "ParentUserFiles",
                column: "PrivateMessageUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_ProjectFaqUserFile_ParentId",
                table: "ParentUserFiles",
                column: "ProjectFaqUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_ProjectIssueUserFile_ParentId",
                table: "ParentUserFiles",
                column: "ProjectIssueUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_ProjectReleaseUserFile_ParentId",
                table: "ParentUserFiles",
                column: "ProjectReleaseUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_ProjectUserFile_ParentId",
                table: "ParentUserFiles",
                column: "ProjectUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_SearchItemUserFile_ParentId",
                table: "ParentUserFiles",
                column: "SearchItemUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_StackExchangeQuestionUserFile_ParentId",
                table: "ParentUserFiles",
                column: "StackExchangeQuestionUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_SurveyUserFile_ParentId",
                table: "ParentUserFiles",
                column: "SurveyUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_UserId",
                table: "ParentUserFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentUserFiles_UserProfileUserFile_ParentId",
                table: "ParentUserFiles",
                column: "UserProfileUserFile_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_AdvertisementCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "AdvertisementCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_AdvertisementUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "AdvertisementUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_AdvertisementVisitor_ParentId",
                table: "ParentVisitors",
                column: "AdvertisementVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_BacklogCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "BacklogCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_BacklogUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "BacklogUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_BacklogVisitor_ParentId",
                table: "ParentVisitors",
                column: "BacklogVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_BlogPostCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "BlogPostCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_BlogPostUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "BlogPostUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_BlogPostVisitor_ParentId",
                table: "ParentVisitors",
                column: "BlogPostVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseQuestionCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseQuestionCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseQuestionUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseQuestionUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseQuestionVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseQuestionVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseTopicCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseTopicCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseTopicUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseTopicUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseTopicVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseTopicVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_CourseVisitor_ParentId",
                table: "ParentVisitors",
                column: "CourseVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_DailyNewsItemCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "DailyNewsItemCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_DailyNewsItemUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "DailyNewsItemUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_LearningPathCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "LearningPathCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_LearningPathUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "LearningPathUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_LearningPathVisitor_ParentId",
                table: "ParentVisitors",
                column: "LearningPathVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ParentId",
                table: "ParentVisitors",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_PrivateMessageCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "PrivateMessageCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_PrivateMessageUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "PrivateMessageUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_PrivateMessageVisitor_ParentId",
                table: "ParentVisitors",
                column: "PrivateMessageVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectFaqCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectFaqCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectFaqUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectFaqUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectFaqVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectFaqVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectIssueCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectIssueCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectIssueUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectIssueUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectIssueVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectIssueVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectReleaseCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectReleaseCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectReleaseUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectReleaseUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectReleaseVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectReleaseVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_ProjectVisitor_ParentId",
                table: "ParentVisitors",
                column: "ProjectVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_SearchItemCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "SearchItemCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_SearchItemUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "SearchItemUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_SearchItemVisitor_ParentId",
                table: "ParentVisitors",
                column: "SearchItemVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_StackExchangeQuestionCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "StackExchangeQuestionCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_StackExchangeQuestionUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "StackExchangeQuestionUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_StackExchangeQuestionVisitor_ParentId",
                table: "ParentVisitors",
                column: "StackExchangeQuestionVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_SurveyCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "SurveyCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_SurveyUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "SurveyUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_SurveyVisitor_ParentId",
                table: "ParentVisitors",
                column: "SurveyVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_UserId",
                table: "ParentVisitors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_UserProfileCommentVisitor_ParentId",
                table: "ParentVisitors",
                column: "UserProfileCommentVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_UserProfileUserFileVisitor_ParentId",
                table: "ParentVisitors",
                column: "UserProfileUserFileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentVisitors_UserProfileVisitor_ParentId",
                table: "ParentVisitors",
                column: "UserProfileVisitor_ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageComments_ParentId",
                table: "PrivateMessageComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageComments_ReplyId",
                table: "PrivateMessageComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageComments_UserId",
                table: "PrivateMessageComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessagePrivateMessageTag_TagsId",
                table: "PrivateMessagePrivateMessageTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_ToUserId",
                table: "PrivateMessages",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessages_UserId",
                table: "PrivateMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageTags_Name",
                table: "PrivateMessageTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateMessageTags_UserId",
                table: "PrivateMessageTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComments_ParentId",
                table: "ProjectComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComments_ReplyId",
                table: "ProjectComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectComments_UserId",
                table: "ProjectComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqComments_ParentId",
                table: "ProjectFaqComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqComments_ReplyId",
                table: "ProjectFaqComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqComments_UserId",
                table: "ProjectFaqComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqProjectFaqTag_TagsId",
                table: "ProjectFaqProjectFaqTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqs_ProjectId",
                table: "ProjectFaqs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqs_UserId",
                table: "ProjectFaqs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqTags_Name",
                table: "ProjectFaqTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFaqTags_UserId",
                table: "ProjectFaqTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueComments_ParentId",
                table: "ProjectIssueComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueComments_ReplyId",
                table: "ProjectIssueComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueComments_UserId",
                table: "ProjectIssueComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssuePriorities_Name",
                table: "ProjectIssuePriorities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssuePriorities_UserId",
                table: "ProjectIssuePriorities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueProjectIssueTag_TagsId",
                table: "ProjectIssueProjectIssueTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssues_IssuePriorityId",
                table: "ProjectIssues",
                column: "IssuePriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssues_IssueStatusId",
                table: "ProjectIssues",
                column: "IssueStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssues_IssueTypeId",
                table: "ProjectIssues",
                column: "IssueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssues_ProjectId",
                table: "ProjectIssues",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssues_UserId",
                table: "ProjectIssues",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueStatuses_Name",
                table: "ProjectIssueStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueStatuses_UserId",
                table: "ProjectIssueStatuses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueTags_Name",
                table: "ProjectIssueTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueTags_UserId",
                table: "ProjectIssueTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueTypes_Name",
                table: "ProjectIssueTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectIssueTypes_UserId",
                table: "ProjectIssueTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProjectTag_TagsId",
                table: "ProjectProjectTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleaseComments_ParentId",
                table: "ProjectReleaseComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleaseComments_ReplyId",
                table: "ProjectReleaseComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleaseComments_UserId",
                table: "ProjectReleaseComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleaseProjectReleaseTag_TagsId",
                table: "ProjectReleaseProjectReleaseTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleases_ProjectId",
                table: "ProjectReleases",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleases_UserId",
                table: "ProjectReleases",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleaseTags_Name",
                table: "ProjectReleaseTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectReleaseTags_UserId",
                table: "ProjectReleaseTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserId",
                table: "Projects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTags_Name",
                table: "ProjectTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTags_UserId",
                table: "ProjectTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_RolesId",
                table: "RoleUser",
                column: "RolesId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchItemComments_ParentId",
                table: "SearchItemComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchItemComments_ReplyId",
                table: "SearchItemComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchItemComments_UserId",
                table: "SearchItemComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchItems_UserId",
                table: "SearchItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchItemSearchItemTag_TagsId",
                table: "SearchItemSearchItemTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchItemTags_Name",
                table: "SearchItemTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SearchItemTags_UserId",
                table: "SearchItemTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestionComments_ParentId",
                table: "StackExchangeQuestionComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestionComments_ReplyId",
                table: "StackExchangeQuestionComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestionComments_UserId",
                table: "StackExchangeQuestionComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestions_QuestionId",
                table: "StackExchangeQuestions",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestions_UserId",
                table: "StackExchangeQuestions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestionStackExchangeQuestionTag_TagsId",
                table: "StackExchangeQuestionStackExchangeQuestionTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestionTags_Name",
                table: "StackExchangeQuestionTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StackExchangeQuestionTags_UserId",
                table: "StackExchangeQuestionTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyComments_ParentId",
                table: "SurveyComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyComments_ReplyId",
                table: "SurveyComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyComments_UserId",
                table: "SurveyComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyItems_SurveyId",
                table: "SurveyItems",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyItemUser_UsersId",
                table: "SurveyItemUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_UserId",
                table: "Surveys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveySurveyTag_TagsId",
                table: "SurveySurveyTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyTags_Name",
                table: "SurveyTags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyTags_UserId",
                table: "SurveyTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileComments_ParentId",
                table: "UserProfileComments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileComments_ReplyId",
                table: "UserProfileComments",
                column: "ReplyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileComments_UserId",
                table: "UserProfileComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EMail",
                table: "Users",
                column: "EMail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FriendlyName",
                table: "Users",
                column: "FriendlyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialNetworks_UserId",
                table: "UserSocialNetworks",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserUsedPasswords_UserId",
                table: "UserUsedPasswords",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementAdvertisementTag");

            migrationBuilder.DropTable(
                name: "AppDataProtectionKeys");

            migrationBuilder.DropTable(
                name: "AppLogItems");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "BacklogBacklogTag");

            migrationBuilder.DropTable(
                name: "BlogPostBlogPostTag");

            migrationBuilder.DropTable(
                name: "BlogPostDrafts");

            migrationBuilder.DropTable(
                name: "CourseCourseTag");

            migrationBuilder.DropTable(
                name: "CourseQuestionCourseQuestionTag");

            migrationBuilder.DropTable(
                name: "CourseTopicCourseTopicTag");

            migrationBuilder.DropTable(
                name: "CourseUser");

            migrationBuilder.DropTable(
                name: "CustomSidebars");

            migrationBuilder.DropTable(
                name: "DailyNewsItemDailyNewsItemTag");

            migrationBuilder.DropTable(
                name: "LearningPathLearningPathTag");

            migrationBuilder.DropTable(
                name: "MassEmails");

            migrationBuilder.DropTable(
                name: "ParentBookmarks");

            migrationBuilder.DropTable(
                name: "ParentReactions");

            migrationBuilder.DropTable(
                name: "ParentVisitors");

            migrationBuilder.DropTable(
                name: "PrivateMessagePrivateMessageTag");

            migrationBuilder.DropTable(
                name: "ProjectFaqProjectFaqTag");

            migrationBuilder.DropTable(
                name: "ProjectIssueProjectIssueTag");

            migrationBuilder.DropTable(
                name: "ProjectProjectTag");

            migrationBuilder.DropTable(
                name: "ProjectReleaseProjectReleaseTag");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "SearchItemSearchItemTag");

            migrationBuilder.DropTable(
                name: "StackExchangeQuestionStackExchangeQuestionTag");

            migrationBuilder.DropTable(
                name: "SurveyItemUser");

            migrationBuilder.DropTable(
                name: "SurveySurveyTag");

            migrationBuilder.DropTable(
                name: "UserSocialNetworks");

            migrationBuilder.DropTable(
                name: "UserUsedPasswords");

            migrationBuilder.DropTable(
                name: "AdvertisementTags");

            migrationBuilder.DropTable(
                name: "BacklogTags");

            migrationBuilder.DropTable(
                name: "BlogPostTags");

            migrationBuilder.DropTable(
                name: "CourseTags");

            migrationBuilder.DropTable(
                name: "CourseQuestionTags");

            migrationBuilder.DropTable(
                name: "CourseTopicTags");

            migrationBuilder.DropTable(
                name: "DailyNewsItemTags");

            migrationBuilder.DropTable(
                name: "LearningPathTags");

            migrationBuilder.DropTable(
                name: "AdvertisementComments");

            migrationBuilder.DropTable(
                name: "BacklogComments");

            migrationBuilder.DropTable(
                name: "BlogPostComments");

            migrationBuilder.DropTable(
                name: "CourseComments");

            migrationBuilder.DropTable(
                name: "CourseQuestionComments");

            migrationBuilder.DropTable(
                name: "CourseTopicComments");

            migrationBuilder.DropTable(
                name: "DailyNewsItemComments");

            migrationBuilder.DropTable(
                name: "LearningPathComments");

            migrationBuilder.DropTable(
                name: "ParentUserFiles");

            migrationBuilder.DropTable(
                name: "PrivateMessageComments");

            migrationBuilder.DropTable(
                name: "ProjectComments");

            migrationBuilder.DropTable(
                name: "ProjectFaqComments");

            migrationBuilder.DropTable(
                name: "ProjectIssueComments");

            migrationBuilder.DropTable(
                name: "ProjectReleaseComments");

            migrationBuilder.DropTable(
                name: "SearchItemComments");

            migrationBuilder.DropTable(
                name: "StackExchangeQuestionComments");

            migrationBuilder.DropTable(
                name: "SurveyComments");

            migrationBuilder.DropTable(
                name: "UserProfileComments");

            migrationBuilder.DropTable(
                name: "PrivateMessageTags");

            migrationBuilder.DropTable(
                name: "ProjectFaqTags");

            migrationBuilder.DropTable(
                name: "ProjectIssueTags");

            migrationBuilder.DropTable(
                name: "ProjectTags");

            migrationBuilder.DropTable(
                name: "ProjectReleaseTags");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SearchItemTags");

            migrationBuilder.DropTable(
                name: "StackExchangeQuestionTags");

            migrationBuilder.DropTable(
                name: "SurveyItems");

            migrationBuilder.DropTable(
                name: "SurveyTags");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Backlogs");

            migrationBuilder.DropTable(
                name: "CourseQuestions");

            migrationBuilder.DropTable(
                name: "CourseTopics");

            migrationBuilder.DropTable(
                name: "DailyNewsItems");

            migrationBuilder.DropTable(
                name: "LearningPaths");

            migrationBuilder.DropTable(
                name: "PrivateMessages");

            migrationBuilder.DropTable(
                name: "ProjectFaqs");

            migrationBuilder.DropTable(
                name: "ProjectIssues");

            migrationBuilder.DropTable(
                name: "ProjectReleases");

            migrationBuilder.DropTable(
                name: "SearchItems");

            migrationBuilder.DropTable(
                name: "StackExchangeQuestions");

            migrationBuilder.DropTable(
                name: "Surveys");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "ProjectIssuePriorities");

            migrationBuilder.DropTable(
                name: "ProjectIssueStatuses");

            migrationBuilder.DropTable(
                name: "ProjectIssueTypes");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
