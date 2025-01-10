using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.ScheduledTasks;

public class DraftsJob(IBlogPostDraftsService blogPostDraftsService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : blogPostDraftsService.RunConvertDraftsToPostsJobAsync(cancellationToken);
}
