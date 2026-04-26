using System.Text;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Services.Contracts;

public interface IEPubExportHtmlProviderService : IScopedService
{
    string ApplyHtmlPageTemplate(string title, string body, string? sideBar);

    Task<string> GetEPubContentItemLinkAsync(WhatsNewItemType type, EPubContentItem subItem);

    void AddHeader(StringBuilder html, string title);

    string WrapInBadge(string title);

    Task<string> GetLastAndNextLinksHtmlAsync(WhatsNewItemType type,
        EPubContentItem item,
        CancellationToken cancellationToken);
}
