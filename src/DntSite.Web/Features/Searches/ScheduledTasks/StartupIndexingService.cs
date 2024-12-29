using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.ScheduledTasks;

public class StartupIndexingService(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<StartupIndexingService> logger,
    ILockerService lockerService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var lockAsync =
            await lockerService.LockAsync<StartupIndexingService>(TimeSpan.FromMinutes(value: 30), stoppingToken);

        try
        {
            await Task.Delay(TimeSpan.FromMinutes(value: 1), stoppingToken);

            logger.LogInformation(message: "{DateTime} Started StartupIndexingService.",
                DateTime.UtcNow.ToString(format: "HH:mm:ss.fff", CultureInfo.InvariantCulture));

            using var scope = serviceScopeFactory.CreateScope();

            var fullTextSearchWriterService = scope.ServiceProvider.GetRequiredService<IFullTextSearchWriterService>();
            await fullTextSearchWriterService.IndexDatabaseAsync(forceStart: false, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to index database");
        }
        finally
        {
            logger.LogInformation(message: "{DateTime} Finished StartupIndexingService.",
                DateTime.UtcNow.ToString(format: "HH:mm:ss.fff", CultureInfo.InvariantCulture));
        }
    }
}
