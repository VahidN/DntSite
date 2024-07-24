using System.Collections.Concurrent;
using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;
using DntSite.Web.Features.Persistence.UnitOfWork;
using Microsoft.Extensions.Options;

namespace DntSite.Web.Features.DbLogger.Services;

public class EfDbLoggerProvider : ILoggerProvider
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly List<EfDbLoggerItem> _currentBatch = new();
    private readonly TimeSpan _interval = TimeSpan.FromSeconds(7);
    private readonly BlockingCollection<EfDbLoggerItem> _messageQueue = new(new ConcurrentQueue<EfDbLoggerItem>());
    private readonly Task _outputTask;
    private readonly IServiceProvider _serviceProvider;
    private readonly IOptions<StartupSettingsModel> _siteSettings;
    private bool _isDisposed;

    public EfDbLoggerProvider(IOptions<StartupSettingsModel> siteSettings, IServiceProvider serviceProvider)
    {
        _siteSettings = siteSettings;
        _serviceProvider = serviceProvider;
        _outputTask = Task.Run(ProcessLogQueueAsync);
    }

    public ILogger CreateLogger(string categoryName)
        => new EfDbLogger(this, _serviceProvider, categoryName, _siteSettings);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            try
            {
                if (disposing)
                {
                    Stop();
                    _messageQueue.Dispose();
                    _cancellationTokenSource.Dispose();
                }
            }
            finally
            {
                _isDisposed = true;
            }
        }
    }

    internal void AddLogItem(EfDbLoggerItem appLogItem)
    {
        if (!_messageQueue.IsAddingCompleted)
        {
            _messageQueue.Add(appLogItem, _cancellationTokenSource.Token);
        }
    }

    [SuppressMessage("Microsoft.Usage", "CA1031:catch a more specific allowed exception type, or rethrow the exception",
        Justification = "cancellation token canceled or CompleteAdding called")]
    private async Task ProcessLogQueueAsync()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            while (_messageQueue.TryTake(out var message))
            {
                try
                {
                    _currentBatch.Add(message);
                }
                catch
                {
                    //cancellation token canceled or CompleteAdding called
                }
            }

            await SaveLogItemsAsync(_currentBatch, _cancellationTokenSource.Token);
            _currentBatch.Clear();

            await Task.Delay(_interval, _cancellationTokenSource.Token);
        }
    }

    [SuppressMessage("Microsoft.Usage", "CA1031:catch a more specific allowed exception type, or rethrow the exception",
        Justification = "don't throw exceptions from logger")]
    private async Task SaveLogItemsAsync(IList<EfDbLoggerItem> items, CancellationToken cancellationToken)
    {
        try
        {
            if (!items.Any())
            {
                return;
            }

            // We need a separate context for the logger to call its SaveChanges several times,
            // without using the current request's context and changing its internal state.
            await _serviceProvider.RunScopedServiceAsync<IUnitOfWork>(async context =>
            {
                foreach (var item in items)
                {
                    context.DbSet<AppLogItem>().Add(item.AppLogItem);
                }

                await context.SaveChangesAsync(cancellationToken);
            });
        }
        catch
        {
            // don't throw exceptions from logger
        }
    }

    [SuppressMessage("Microsoft.Usage", "CA1031:catch a more specific allowed exception type, or rethrow the exception",
        Justification = "don't throw exceptions from logger")]
    private void Stop()
    {
        _cancellationTokenSource.Cancel();
        _messageQueue.CompleteAdding();

        try
        {
            _outputTask.Wait(_interval);
        }
        catch
        {
            // don't throw exceptions from logger
        }
    }
}
