namespace DntSite.Web.Features.Stats.Services.Contracts;

public interface ISpidersService : ISingletonService
{
    public void AddCaughtScraper(string ip, string ua);

    public Task<bool> IsSpiderClientAsync(string ip, string ua);

    public Task<bool> IsSpiderClientAsync(HttpContext? httpContext);
}
