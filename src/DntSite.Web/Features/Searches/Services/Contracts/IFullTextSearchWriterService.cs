namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface IFullTextSearchWriterService : IScopedService
{
    public Task IndexDatabaseAsync(bool forceStart, CancellationToken stoppingToken);
}
