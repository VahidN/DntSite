using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.ScheduledTasks.Services;

public class NewsHttpStatusCodeJob(IDailyNewsItemsService dailyNewsItemsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown ? Task.CompletedTask : dailyNewsItemsService.UpdateAllNewsLastHttpStatusCodeAsync();

    public bool IsShuttingDown { get; set; }
}
