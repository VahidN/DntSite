using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.SiteBackup.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Stats.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class RecalculatePostsCount
{
    [InjectComponentScoped] internal IStatService StatService { get; set; } = null!;

    [InjectComponentScoped] internal ISiteReferrersService SiteReferrersService { get; set; } = null!;

    [InjectComponentScoped] internal IPdfExportService PdfExportService { get; set; } = null!;

    [InjectComponentScoped] internal IAIDailyNewsService AIDailyNewsService { get; set; } = null!;

    [InjectComponentScoped] internal IDailyNewsScreenshotsService DailyNewsScreenshotsService { get; set; } = null!;

    [InjectComponentScoped] internal IWebSiteBackupService WebSiteBackupService { get; set; } = null!;

    [Inject] internal IFullTextSearchService FullTextSearchService { get; set; } = null!;

    [Inject] internal ILogger<RecalculatePostsCount> Logger { get; set; } = null!;

    [SupplyParameterFromForm] internal RecalculatePostsCountAction RecalculateAction { get; set; }

    [CascadingParameter] internal DntAlert Alert { get; set; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { get; set; } = null!;

    [InjectComponentScoped] internal IBlogPostsPdfExportService BlogPostsPdfExportService { get; set; } = null!;

    [InjectComponentScoped] internal ICourseTopicsPdfExportService CourseTopicsPdfExportService { get; set; } = null!;

    [InjectComponentScoped] internal IQuestionsPdfExportService QuestionsPdfExportService { get; set; } = null!;

    [InjectComponentScoped] internal IDailyNewsPdfExportService DailyNewsPdfExportService { get; set; } = null!;

    [InjectComponentScoped] internal IEPubExportService EPubExportService { get; set; } = null!;

    private async Task OnValidSubmitAsync()
    {
        Logger.LogWarning(message: "Performing {Action}.", RecalculateAction);

        switch (RecalculateAction)
        {
            case RecalculatePostsCountAction.RecalculateForm:
                await StatService.RecalculateAllBlogPostsCommentsCountsAsync();
                await StatService.RecalculateTagsInUseCountsAsync<BlogPostTag, BlogPost>();
                await StatService.RecalculateAllUsersNumberOfPostsAndCommentsAsync();

                break;
            case RecalculatePostsCountAction.UpdateAllUsersRatings:
                await StatService.UpdateAllUsersRatingsAsync();

                break;
            case RecalculatePostsCountAction.UpdateFullTextIndex:
                FullTextSearchService.DeleteOldIndexFiles();

                break;

            case RecalculatePostsCountAction.DeleteAllSiteReferrers:
                await SiteReferrersService.DeleteAllAsync();

                break;

            case RecalculatePostsCountAction.InvalidateAllYoutubeScreenshots:
                await DailyNewsScreenshotsService.InvalidateAllYoutubeScreenshotsAsync();

                break;

            case RecalculatePostsCountAction.InvalidateAllScreenshots:
                await DailyNewsScreenshotsService.InvalidateAllScreenshotsAsync();

                break;

            case RecalculatePostsCountAction.TryReDownloadFailedScreenshots:
                await DailyNewsScreenshotsService.TryReDownloadFailedScreenshotsAsync();

                break;

            case RecalculatePostsCountAction.RebuildExports:
                PdfExportService.RebuildExports();

                break;

            case RecalculatePostsCountAction.DeleteLargeHtmlFiles:
                PdfExportService.DeleteLargeHtmlTagFiles();

                break;

            case RecalculatePostsCountAction.RunAIDailyNewsService:
                await AIDailyNewsService.StartProcessingNewsFeedsAsync();

                break;

            case RecalculatePostsCountAction.RunBackup:
                await WebSiteBackupService.CreateDatabaseBackupAsync();

                break;

            case RecalculatePostsCountAction.CreateEPub:
                await QuestionsPdfExportService.ExportNotProcessedQuestionsToSeparatePdfFilesAsync(ExportType.HtmlFile);

                await CourseTopicsPdfExportService.ExportNotProcessedCourseTopicsToSeparatePdfFilesAsync(ExportType
                    .HtmlFile);

                await BlogPostsPdfExportService.ExportNotProcessedBlogPostsToSeparatePdfFilesAsync(ExportType.HtmlFile);

                await DailyNewsPdfExportService.ExportNotProcessedDailyNewsToSeparatePdfFilesAsync(ExportType.HtmlFile);

                await EPubExportService.StartAsync();

                break;
        }

        Alert.ShowAlert(AlertType.Success, title: "با تشکر!", message: "عملیات محاسبات مجدد، انجام شد.");
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..StatsBreadCrumbs.DefaultBreadCrumbs]);
}
