using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Services.Contracts;

public interface IEPubExportDocsInfoService : IScopedService
{
    Task<(string? Title, string? FileName)> GetNextLastItemsAsync(WhatsNewItemType type,
        EPubContentItem item,
        bool isAscending,
        int numberOfTries,
        CancellationToken cancellationToken);

    string GetEPubTocPath(string domain, int page);

    string GetArticlesTocPath(string domain, int page);

    string GetAuthorsTocPath(string domain, int page);

    string GetTagsTocPath(string domain, int page);

    string GetLearningPathsTocPath(string domain, int page);

    string GetCoursesTocPath(string domain, int page);

    string GetNewsTocPath(string domain, int page);

    Task<(string? Content, string? Title, string? FileName)> GetDocInfoAsync(WhatsNewItemType type,
        int itemId,
        CancellationToken cancellationToken);

    Task<(string? DocPath, string? FileName)> GetDocPathAsync(WhatsNewItemType type, int id);

    string GetEbookFilePath();
}
