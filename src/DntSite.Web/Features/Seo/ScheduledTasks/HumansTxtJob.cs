using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Seo.ScheduledTasks;

public class HumansTxtJob(IHumansTxtFileService humansTxtFileService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : humansTxtFileService.CreateHumansTxtFileAsync(cancellationToken);
}
