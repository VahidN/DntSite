using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Courses.Services.Contracts;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.Posts.Services.Contracts;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Searches.Services;

public class FullTextSearchWriterService(
    IFullTextSearchService fullTextSearchService,
    ILogger<FullTextSearchWriterService> logger,
    IAdvertisementCommentsService advertisementCommentsService,
    IAdvertisementsService advertisementsService,
    IBacklogsService backlogsService,
    ICourseTopicCommentsService courseTopicCommentsService,
    ICourseTopicsService courseTopicsService,
    ICoursesService coursesService,
    IDailyNewsItemCommentsService dailyNewsItemCommentsService,
    IDailyNewsItemsService dailyNewsItemsService,
    IBlogCommentsService blogCommentsService,
    IBlogPostsService blogPostsService,
    IProjectFaqsService projectFaqsService,
    IProjectIssueCommentsService projectIssueCommentsService,
    IProjectIssuesService projectIssuesService,
    IProjectReleasesService projectReleasesService,
    IProjectsService projectsService,
    ILearningPathService learningPathService,
    IQuestionsCommentsService questionsCommentsService,
    IQuestionsService questionsService,
    IVoteCommentsService voteCommentsService,
    IVotesService votesService,
    ILockerService lockerService) : IFullTextSearchWriterService
{
    public async Task IndexDatabaseAsync(bool forceStart, CancellationToken stoppingToken)
    {
        using var @lock = await lockerService.LockAsync<FullTextSearchWriterService>(stoppingToken);

        try
        {
            if (!forceStart && fullTextSearchService.IsDatabaseIndexed)
            {
                return;
            }

            logger.LogWarning(message: "Started Indexing Database");

            Func<Task>[] actions =
            [
                advertisementCommentsService.IndexAdvertisementCommentsAsync,
                advertisementsService.IndexAdvertisementsAsync, backlogsService.IndexBackLogsAsync,
                courseTopicCommentsService.IndexCourseTopicCommentsAsync,
                courseTopicsService.IndexCourseTopicsAsync, coursesService.IndexCoursesAsync,
                dailyNewsItemCommentsService.IndexDailyNewsItemCommentsAsync,
                dailyNewsItemsService.IndexDailyNewsItemsAsync, blogCommentsService.IndexBlogPostCommentsAsync,
                blogPostsService.IndexBlogPostsAsync, projectFaqsService.IndexProjectFaqsAsync,
                projectIssueCommentsService.IndexProjectIssueCommentsAsync,
                projectIssuesService.IndexProjectIssuesAsync, projectReleasesService.IndexProjectReleasesAsync,
                projectsService.IndexProjectsAsync, learningPathService.IndexLearningPathsAsync,
                questionsCommentsService.IndexStackExchangeQuestionCommentsAsync,
                questionsService.IndexStackExchangeQuestionsAsync, voteCommentsService.IndexSurveyCommentsAsync,
                votesService.IndexSurveysAsync
            ];

            for (var index = 0; index < actions.Length; index++)
            {
                var action = actions[index];

                logger.LogInformation(message: "Indexing[{Index}/{Total}] {Type}", index, actions.Length,
                    action.Method.Name);

                await action();
                await Task.Delay(TimeSpan.FromSeconds(value: 7), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to index database");
        }

        logger.LogWarning(message: "Finished Indexing Database");
    }
}
