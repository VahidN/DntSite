﻿using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.ScheduledTasks;

public class DeleteOrphansJob(IBlogPostDraftsService blogPostDraftsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown ? Task.CompletedTask : blogPostDraftsService.DeleteConvertedBlogPostDraftsAsync();

    public bool IsShuttingDown { get; set; }
}
