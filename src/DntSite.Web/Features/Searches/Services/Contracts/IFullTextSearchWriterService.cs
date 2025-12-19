namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface IFullTextSearchWriterService : IScopedService
{
    Task IndexDatabaseAsync(bool forceStart, CancellationToken stoppingToken);
}
