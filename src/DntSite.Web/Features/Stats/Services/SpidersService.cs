using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Services;

public class SpidersService(IUAParserService uaParserService) : ISpidersService
{
    private readonly ConcurrentDictionaryLocked<string, string> _caughtScrapers = new(StringComparer.OrdinalIgnoreCase);

    public void AddCaughtScraper(string ip, string ua) => _caughtScrapers.LockedAddOrUpdate(ip, ua, (_, _) => ua);

    public async Task<bool> IsSpiderClientAsync(string ip, string ua)
        => _caughtScrapers.ContainsKey(ip) || await uaParserService.IsSpiderClientAsync(ua);

    public Task<bool> IsSpiderClientAsync(HttpContext? httpContext)
        => IsSpiderClientAsync(httpContext?.GetIP() ?? "::1", httpContext?.GetUserAgent() ?? "Unknown");
}
