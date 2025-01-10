using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class UpdateDeletedNewsHttpStatusCodeJob(IDailyNewsItemsService dailyNewsItemsService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : dailyNewsItemsService.UpdateAllNewsLastHttpStatusCodeAsync(UpdateNewsStatusAction.UpdateDeletesOnes,
                cancellationToken);
}
