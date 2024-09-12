using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.ScheduledTasks;

public class DraftsJob(IBlogPostDraftsService blogPostDraftsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown ? Task.CompletedTask : blogPostDraftsService.RunConvertDraftsToPostsJobAsync();

    public bool IsShuttingDown { get; set; }
}
