using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class NewsHttpStatusCodeJob(IDailyNewsItemsService dailyNewsItemsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown ? Task.CompletedTask : dailyNewsItemsService.UpdateAllNewsLastHttpStatusCodeAsync();

    public bool IsShuttingDown { get; set; }
}
