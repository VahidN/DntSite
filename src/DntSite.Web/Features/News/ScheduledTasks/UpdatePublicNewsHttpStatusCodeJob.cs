using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class UpdatePublicNewsHttpStatusCodeJob(IDailyNewsItemsService dailyNewsItemsService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown
            ? Task.CompletedTask
            : dailyNewsItemsService.UpdateAllNewsLastHttpStatusCodeAsync(UpdateNewsStatusAction.UpdatePublicOnes);

    public bool IsShuttingDown { get; set; }
}
