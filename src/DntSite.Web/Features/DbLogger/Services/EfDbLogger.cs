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

        if (!message.IsEmpty())
        {
            message = message.ContainsHtmlTags() ? message : $"<code>{message}</code>";
        }

        if (exception is not null)
        {
            message = $"{message}<br/><br/><code>{exception.Demystify()}</code>";
        }

        if (message.IsEmpty())
        {
            return;
        }

        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        var httpContext = httpContextAccessor?.HttpContext;

        if (httpContext?.Request is not null && !HasRequestLog(message))
        {
            var requestLog = httpContext.Request.LogRequest(httpContext.Response?.StatusCode);
            message = $"{message}<br/><br/>{requestLog}";
        }

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

    private static bool HasRequestLog(string message)
        => message.Contains(value: "Request Info", StringComparison.Ordinal);

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
