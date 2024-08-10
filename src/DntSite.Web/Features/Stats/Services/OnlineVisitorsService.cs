using System.Collections.Concurrent;
using System.Timers;
using DntSite.Web.Features.Stats.Services.Contracts;
using Timer = System.Timers.Timer;

namespace DntSite.Web.Features.Stats.Services;

public class OnlineVisitorsService : IOnlineVisitorsService
{
    private const int Interval = 5; // 5 minutes
    private const int ReleaseInterval = Interval * 60 * 1000; // 5 minutes
    private readonly Timer _timer = new();

    private readonly ConcurrentDictionary<string, DateTime> _visitors = new(StringComparer.Ordinal);
    private bool _isDisposed;

    public OnlineVisitorsService() => CreateTimer();

    public int OnlineVisitorsCount => _visitors.Count;

    public void UpdateStat(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var ip = context.GetIP();

        if (!string.IsNullOrWhiteSpace(ip))
        {
            _visitors[ip] = DateTime.UtcNow;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void CreateTimer()
    {
        _timer.Interval = ReleaseInterval;
        _timer.Start();
        _timer.Elapsed += TimerElapsed;
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_visitors.IsEmpty)
        {
            return;
        }

        var oldItems = _visitors.Where(x => x.Value < DateTime.UtcNow.AddMinutes(-Interval)).ToList();

        foreach (var item in oldItems)
        {
            _visitors.TryRemove(item.Key, out _);
        }
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

            _timer.Enabled = false;
            _timer.Stop();
            _timer.Dispose();
        }
        finally
        {
            _isDisposed = true;
        }
    }
}
