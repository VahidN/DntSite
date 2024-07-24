using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.ScheduledTasks.Services;

public class DeleteOrphans(IBlogPostDraftsService blogPostDraftsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown ? Task.CompletedTask : blogPostDraftsService.DeleteConvertedBlogPostDraftsAsync();

    public bool IsShuttingDown { get; set; }
}
