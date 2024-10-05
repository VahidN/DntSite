using System.Collections.Concurrent;
using DntSite.Web.Features.Stats.Middlewares.Contracts;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Middlewares;

public class SiteReferrersMiddleware : IMiddleware, ISingletonService, IDisposable
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly List<SiteReferrerItem> _currentBatch = [];

    private readonly TimeSpan _interval = TimeSpan.FromSeconds(value: 7);
    private readonly ILogger<SiteReferrersMiddleware> _logger;
    private readonly BlockingCollection<SiteReferrerItem> _messageQueue = new(new ConcurrentQueue<SiteReferrerItem>());

    private readonly Task _outputTask;
    private readonly IServiceProvider _serviceProvider;
    private readonly IUAParserService _uaParserService;

    private bool _isDisposed;

    public SiteReferrersMiddleware(IServiceProvider serviceProvider,
        IUAParserService uaParserService,
        ILogger<SiteReferrersMiddleware> logger)
    {
        _serviceProvider = serviceProvider;
        _uaParserService = uaParserService;
        _logger = logger;
        _outputTask = Task.Run(ProcessItemsQueueAsync);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var rootUrl = context.GetBaseUrl();
        var referrerUrl = context.GetReferrerUrl();
        var destinationUrl = context.GetRawUrl();

        try
        {
            if (!await ShouldSkipThisRequestAsync(context, referrerUrl, destinationUrl, rootUrl))
            {
                var isLocalReferrer = referrerUrl.IsLocalReferrer(destinationUrl);
                AddSiteReferrerItemToQueue(new SiteReferrerItem(referrerUrl, destinationUrl, isLocalReferrer));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(),
                message:
                "SiteReferrers Error -> RootUrl: {RootUrl}, ReferrerUrl: {ReferrerUrl}, DestinationUrl: {DestinationUrl}, Log: {Log}",
                rootUrl, referrerUrl, destinationUrl, context.Request.LogRequest(responseCode: 500));
        }

        await next(context);
    }

    private async Task<bool>
        ShouldSkipThisRequestAsync(HttpContext context, string referrerUrl, string destinationUrl, string rootUrl)
        => string.IsNullOrEmpty(referrerUrl) ||
           string.Equals(referrerUrl, destinationUrl, StringComparison.OrdinalIgnoreCase) ||
           !referrerUrl.IsValidUrl() || context.IsProtectedRoute() ||
           await _uaParserService.IsSpiderClientAsync(context) || !destinationUrl.IsReferrerToThisSite(rootUrl) ||
           destinationUrl.IsStaticFileUrl() || DoNotLog(context);

    private static bool DoNotLog(HttpContext context)
        => context.GetEndpoint()?.Metadata?.GetMetadata<DoNotLogReferrerAttribute>() is not null;

    private async Task ProcessItemsQueueAsync()
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

            await ProcessItemsAsync(_currentBatch, _cancellationTokenSource.Token);
            _currentBatch.Clear();

            await Task.Delay(_interval, _cancellationTokenSource.Token);
        }
    }

    private async Task ProcessItemsAsync(IList<SiteReferrerItem> items, CancellationToken cancellationToken)
    {
        try
        {
            if (!items.Any())
            {
                return;
            }

            // We need a separate context for the logger to call its SaveChanges several times,
            // without using the current request's context and changing its internal state.
            await _serviceProvider.RunScopedServiceAsync<ISiteReferrersService>(async siteReferrersService =>
            {
                foreach (var item in items)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    await siteReferrersService.TryAddOrUpdateReferrerAsync(item.ReferrerUrl, item.DestinationUrl,
                        item.IsLocalReferrer);
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Demystify(), message: "TryAddOrUpdateReferrerAsync");
        }
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

    private void AddSiteReferrerItemToQueue(SiteReferrerItem referrerItem)
    {
        if (!_messageQueue.IsAddingCompleted)
        {
            _messageQueue.Add(referrerItem, _cancellationTokenSource.Token);
        }
    }

    private sealed record SiteReferrerItem(string ReferrerUrl, string DestinationUrl, bool IsLocalReferrer);
}
