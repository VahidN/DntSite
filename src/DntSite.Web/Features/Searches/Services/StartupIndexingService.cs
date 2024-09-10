using AsyncKeyedLock;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.Searches.Services;

public class StartupIndexingService(IServiceScopeFactory serviceScopeFactory, ILogger<StartupIndexingService> logger)
    : BackgroundService
{
    private static readonly AsyncNonKeyedLocker Lock = new(maxCount: 1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var lockAsync = await Lock.LockAsync(stoppingToken);

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

    public override void Dispose()
    {
        base.Dispose();
        Lock.Dispose();
    }
}
