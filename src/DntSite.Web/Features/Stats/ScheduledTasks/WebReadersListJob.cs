using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.ScheduledTasks;

public class WebReadersListJob(IStatService statService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : statService.UpdateAllUsersRatingsAsync(cancellationToken);
}
