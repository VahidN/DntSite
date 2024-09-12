using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Seo.ScheduledTasks;

public class HumansTxtJob(IHumansTxtFileService humansTxtFileService) : IScheduledTask
{
    public Task RunAsync() => IsShuttingDown ? Task.CompletedTask : humansTxtFileService.CreateHumansTxtFileAsync();

    public bool IsShuttingDown { get; set; }
}
