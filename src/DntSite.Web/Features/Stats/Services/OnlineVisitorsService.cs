using System.Collections.Concurrent;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Services;

namespace DntSite.Web.Features.Stats.Services;

public class OnlineVisitorsService : IOnlineVisitorsService
{
    public const int PurgeInterval = 10; // 10 minutes

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly List<OnlineVisitorInfoModel> _currentBatch = [];

    private readonly TimeSpan _interval = TimeSpan.FromSeconds(value: 7);
    private readonly ILogger<OnlineVisitorsService> _logger;

    private readonly BlockingCollection<OnlineVisitorInfoModel> _messageQueue =
        new(new ConcurrentQueue<OnlineVisitorInfoModel>());

    private readonly Task _outputTask;
    private readonly ISitePageTitlesCacheService _sitePageTitlesCacheService;
    private readonly IUAParserService _uaParserService;

    private readonly List<OnlineVisitorInfoModel> _visitors = [];
    private bool _isDisposed;

    public OnlineVisitorsService(IUAParserService uaParserService,
        ISitePageTitlesCacheService sitePageTitlesCacheService,
        ILogger<OnlineVisitorsService> logger)
    {
        _uaParserService = uaParserService;
        _sitePageTitlesCacheService = sitePageTitlesCacheService;
        _logger = logger;
        _outputTask = Task.Run(ProcessItemsQueueAsync);
    }

    public async Task UpdateStatAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var ip = context.GetIP();

        if (string.IsNullOrWhiteSpace(ip))
        {
            return;
        }

        var ua = context.GetUserAgent() ?? "Unknown";
        var isSpider = await _uaParserService.IsSpiderClientAsync(ua);
        var clientInfo = await _uaParserService.GetClientInfoAsync(context);
        var referrerUrl = context.GetReferrerUrl();

        AddItemToQueue(new OnlineVisitorInfoModel
        {
            Ip = ip,
            VisitTime = DateTime.UtcNow,
            IsSpider = isSpider,
            UserAgent = ua,
            ReferrerUrl = referrerUrl,
            ReferrerUrlTitle = referrerUrl,
            VisitedUrl = context.GetRawUrl(),
            IsProtectedPage = context.IsProtectedRoute(),
            RootUrl = context.GetBaseUrl(),
            ClientInfo = clientInfo,
            DisplayName = context.User.GetFirstUserClaimValue(UserRolesService.DisplayNameClaim)
        });
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public PagedResultModel<OnlineVisitorInfoModel> GetPagedOnlineVisitorsList(int pageNumber,
        int recordsPerPage,
        bool isSpider)
    {
        var skipRecords = pageNumber * recordsPerPage;

        var query = _visitors.Where(x
                => x.IsSpider == isSpider && x is
                {
                    IsStaticFileUrl: false, IsProtectedPage: false, HasMissingVisitedUrlTitle: false
                })
            .ToList();

        var items = query.OrderByDescending(x => x.VisitTime).Skip(skipRecords).Take(recordsPerPage).ToList();

        return new PagedResultModel<OnlineVisitorInfoModel>
        {
            TotalItems = query.Count,
            Data = items
        };
    }

    public OnlineVisitorsInfoModel GetOnlineVisitorsInfo()
        => new()
        {
            TotalOnlineAuthenticatedUsersCount =
                _visitors.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName) && !x.IsSpider)
                    .DistinctBy(x => x.Ip)
                    .Count(),
            TotalOnlineGuestUsersCount =
                _visitors.Where(x => string.IsNullOrWhiteSpace(x.DisplayName) && !x.IsSpider)
                    .DistinctBy(x => x.Ip)
                    .Count(),
            OnlineSpidersCount = _visitors.Where(x => x.IsSpider).DistinctBy(x => x.Ip).Count(),
            TotalOnlineVisitorsCount = _visitors.DistinctBy(x => x.Ip).Count()
        };

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

    private async Task ProcessItemsAsync(List<OnlineVisitorInfoModel> items, CancellationToken cancellationToken)
    {
        try
        {
            await AddNewItemsAsync(items, cancellationToken);
            RemoveOldItems();
            TryFixMissingLocalUrlsTitles();
            TryFixMissingReferrerUrlsTitles();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, message: "ProcessItemsAsync");
        }
    }

    private void TryFixMissingReferrerUrlsTitles()
    {
        if (_visitors.Count == 0)
        {
            return;
        }

        foreach (var visitor in _visitors.Where(visitor => visitor.HasMissingReferrerUrlTitle))
        {
            visitor.ReferrerUrlTitle = _sitePageTitlesCacheService.GetPageTitle(visitor.ReferrerUrl);
        }
    }

    private void TryFixMissingLocalUrlsTitles()
    {
        if (_visitors.Count == 0)
        {
            return;
        }

        foreach (var visitor in _visitors.Where(visitor => visitor.HasMissingVisitedUrlTitle))
        {
            visitor.VisitedUrlTitle = _sitePageTitlesCacheService.GetPageTitle(visitor.VisitedUrl);
        }
    }

    private async Task AddNewItemsAsync(List<OnlineVisitorInfoModel> items, CancellationToken cancellationToken)
    {
        foreach (var item in items)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            item.VisitedUrlTitle =
                await _sitePageTitlesCacheService.GetOrAddSitePageTitleAsync(item.VisitedUrl, fetchUrl: false);

            _visitors.Add(item);
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

    private void AddItemToQueue(OnlineVisitorInfoModel item)
    {
        if (!_messageQueue.IsAddingCompleted)
        {
            _messageQueue.Add(item, _cancellationTokenSource.Token);
        }
    }

    private void RemoveOldItems()
    {
        if (_visitors.Count == 0)
        {
            return;
        }

        var purgeDateTime = DateTime.UtcNow.AddMinutes(-PurgeInterval);
        _visitors.RemoveAll(visitor => visitor.VisitTime < purgeDateTime);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        try
        {
            if (!disposing)
            {
                return;
            }

            Stop();
            _messageQueue.Dispose();
            _cancellationTokenSource.Dispose();
        }
        finally
        {
            _isDisposed = true;
        }
    }
}
