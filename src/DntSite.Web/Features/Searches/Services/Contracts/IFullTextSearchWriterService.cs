namespace DntSite.Web.Features.Searches.Services.Contracts;

public interface IFullTextSearchWriterService : IScopedService
{
    Task IndexDatabaseAsync(CancellationToken stoppingToken);
}
