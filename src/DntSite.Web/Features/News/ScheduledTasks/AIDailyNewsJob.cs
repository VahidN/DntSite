using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.ScheduledTasks;

public class AIDailyNewsJob(IAIDailyNewsService aiDailyNewsService) : IScheduledTask
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        await aiDailyNewsService.StartProcessingNewsFeedsAsync(cancellationToken);
    }
}
