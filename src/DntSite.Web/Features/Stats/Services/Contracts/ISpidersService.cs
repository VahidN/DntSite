namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISpidersService : ISingletonService
{
    void AddCaughtScraper(string ip, string ua);

    Task<bool> IsSpiderClientAsync(string ip, string ua);

    Task<bool> IsSpiderClientAsync(HttpContext? httpContext);
}
