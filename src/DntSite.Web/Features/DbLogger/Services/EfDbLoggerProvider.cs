using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.DbLogger.Services;

public class EfDbLoggerProvider : ILoggerProvider
{
    private readonly IBackgroundQueueService _backgroundQueueService;
    private readonly IServiceProvider _serviceProvider;
    private StartupSettingsModel _siteSettings;

    public EfDbLoggerProvider(IOptionsMonitor<StartupSettingsModel> siteSettings,
        IServiceProvider serviceProvider,
        IBackgroundQueueService backgroundQueueService)
    {
        ArgumentNullException.ThrowIfNull(siteSettings);

        _siteSettings = siteSettings.CurrentValue;
        siteSettings.OnChange(settings => _siteSettings = settings);

        _serviceProvider = serviceProvider;
        _backgroundQueueService = backgroundQueueService;
    }

    public ILogger CreateLogger(string categoryName)
        => new EfDbLogger(this, _serviceProvider, categoryName, _siteSettings);

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // empty on purpose
        }
    }

    internal void AddLogItem(EfDbLoggerItem appLogItem)
        => _backgroundQueueService.QueueBackgroundWorkItem(async (cancellationToken, serviceProvider) =>
        {
            try
            {
                // We need a separate context for the logger to call its SaveChanges several times,
                // without using the current request's context and changing its internal state.
                using var uow = serviceProvider.GetRequiredService<IUnitOfWork>();
                uow.DbSet<AppLogItem>().Add(appLogItem.AppLogItem);
                await uow.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // don't throw exceptions from logger
                WriteLine(appLogItem.AppLogItem.Message);
                WriteLine(ex);
            }
        });
}
