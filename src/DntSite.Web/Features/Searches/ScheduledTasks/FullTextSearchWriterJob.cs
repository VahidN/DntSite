using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.ScheduledTasks;

public class FullTextSearchWriterJob(IFullTextSearchWriterService fullTextSearchWriterService) : IScheduledTask
{
    public Task RunAsync(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested
            ? Task.CompletedTask
            : fullTextSearchWriterService.IndexDatabaseAsync(forceStart: false, cancellationToken);
}
