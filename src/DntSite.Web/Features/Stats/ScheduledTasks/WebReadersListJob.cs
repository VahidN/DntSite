using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.ScheduledTasks;

public class WebReadersListJob(IStatService statService) : IScheduledTask
{
    public Task RunAsync() => IsShuttingDown ? Task.CompletedTask : statService.UpdateAllUsersRatingsAsync();

    public bool IsShuttingDown { get; set; }
}
