namespace DntSite.Web.Features.Common.Services.Contracts;

public interface ISitePageTitlesCacheService : ISingletonService
{
    Task<string?> GetOrAddSitePageTitleAsync(string? url, bool fetchUrl);

    string? GetPageTitle(string? url);

    void AddSitePageTitle(string? url, string? title);
}
