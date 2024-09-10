using DntSite.Web.Features.AppConfigs.Entities;
using DntSite.Web.Features.AppConfigs.Models;

namespace DntSite.Web.Features.DbLogger.Services;

public class EfDbLogger(
    EfDbLoggerProvider loggerProvider,
    IServiceProvider serviceProvider,
    string loggerName,
    StartupSettingsModel siteSettings) : ILogger
{
    private readonly LogLevel _minLevel = siteSettings.Logging.LogLevel.Default;

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
        => new NoopDisposable();

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLevel;

    public void Log<TState>(LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(formatter);

        var message = formatter(state, exception);

        if (exception is not null)
        {
            message = $"{message}{Environment.NewLine}{exception.Demystify()}";
        }

        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        var httpContext = httpContextAccessor?.HttpContext;

        var appLogItem = new AppLogItem
        {
            Url = httpContext.GetCurrentUrl(),
            EventId = eventId.Id,
            LogLevel = logLevel.ToString(),
            Logger = loggerName,
            Message = message,
            UserId = httpContext?.User?.GetUserId(),
            Audit =
            {
                CreatedByUserAgent = httpContext?.GetUserAgent() ?? "Program",
                CreatedByUserIp = httpContext?.GetIP() ?? "::1"
            }
        };

        loggerProvider.AddLogItem(new EfDbLoggerItem
        {
            AppLogItem = appLogItem
        });
    }

    private sealed class NoopDisposable : IDisposable
    {
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private static void Dispose(bool disposing)
        {
            if (disposing)
            {
                // empty on purpose
            }
        }
    }
}
