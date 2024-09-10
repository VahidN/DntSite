using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.ScheduledTasks.Services;

public class FullTextSearchWriterJob(IFullTextSearchWriterService fullTextSearchWriterService) : IScheduledTask
{
    public Task RunAsync()
        => IsShuttingDown
            ? Task.CompletedTask
            : fullTextSearchWriterService.IndexDatabaseAsync(forceStart: false, stoppingToken: default);

    public bool IsShuttingDown { get; set; }
}
