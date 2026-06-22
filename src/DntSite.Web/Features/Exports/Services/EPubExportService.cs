using System.Text;
using DntSite.Web.Features.AppConfigs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;
using DntSite.Web.Features.SiteBackup.Services.Contracts;

namespace DntSite.Web.Features.Exports.Services;

public class EPubExportService(
    IEPubExportDocsInfoService docsInfoService,
    IEPubExportHtmlProviderService htmlProviderService,
    IEPubExportDataProviderService ePubExportDataProviderService,
    IAppFoldersService appFoldersService,
    ICachedAppSettingsProvider cachedAppSettingsProvider,
    ILearningPathPdfExportsService learningPathPdfExportsService,
    IWebSiteBackupService webSiteBackupService,
    ILogger<EPubExportService> logger) : IEPubExportService
{
    private List<EPubListItem>? _allCourses;

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var (baseUrl, domain) = await cachedAppSettingsProvider.GetSiteRootDomainAsync();

            if (baseUrl.IsEmpty() || domain.IsEmpty())
            {
                if (logger.IsEnabled(LogLevel.Critical))
                {
                    logger.LogCritical(message: "`SiteRoot` URL is not set in the app settings.");
                }

                return;
            }

            var tocItems = await ePubExportDataProviderService.GetEPubTocItemsAsync(cancellationToken);

            if (tocItems is { ArticlesCount: 0, NewsCount: 0 })
            {
                if (logger.IsEnabled(LogLevel.Warning))
                {
                    logger.LogWarning(message: "There is no content to produce the .EPUB file.");
                }

                return;
            }

            var ebookFilePath = await GenerateEPubAsync(tocItems, baseUrl, domain, cancellationToken);
            await Task.Delay(TimeSpan.FromMilliseconds(value: 500), cancellationToken);

            await webSiteBackupService.UploadSiteEPubFileAsync(ebookFilePath, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Demystify(), message: "Failed to UploadSiteEPubFile.");
        }
    }

    private async Task<string> GenerateEPubAsync(EPubTocItems tocItems,
        string baseUrl,
        string domain,
        CancellationToken cancellationToken)
    {
        var ebookFilePath = docsInfoService.GetEbookFilePath();
        using var epub = new EPubDocument(ebookFilePath);
        AddMetaData(epub, domain);
        AddStaticAssets(epub);
        var sideBar = CreateMainToc(epub, tocItems, baseUrl, domain);
        await AddTableOfContentsAsync(epub, domain, sideBar, cancellationToken);

        await AddContentsAsync(epub, WhatsNewItemType.Posts, processSubItems: false, domain, sideBar,
            cancellationToken);

        await AddContentsAsync(epub, WhatsNewItemType.AllCoursesTopics, processSubItems: true, domain, sideBar,
            cancellationToken);

        await AddContentsAsync(epub, WhatsNewItemType.News, processSubItems: false, domain, sideBar, cancellationToken);
        epub.Generate();

        return ebookFilePath;
    }

    private async Task AddTableOfContentsAsync(EPubDocument epub,
        string domain,
        string sideBar,
        CancellationToken cancellationToken)
    {
        await CreateItemsListContentAsync(epub, title: "مطالب",
            page => docsInfoService.GetArticlesTocPath(domain, page), itemsPerPage: 100,
            (page, itemsPerPage)
                => ePubExportDataProviderService.GetArticlesAsync(page, itemsPerPage, cancellationToken),
            pagerPage => docsInfoService.GetArticlesTocPath(domain, pagerPage), WhatsNewItemType.Posts,
            fixLocalUrls: false, domain, sideBar, cancellationToken);

        await CreateItemsListContentAsync(epub, title: "دوره‌ها",
            page => docsInfoService.GetCoursesTocPath(domain, page), itemsPerPage: 10,
            (page, itemsPerPage)
                => ePubExportDataProviderService.GetCoursesAsync(page, itemsPerPage, cancellationToken),
            pagerPage => docsInfoService.GetCoursesTocPath(domain, pagerPage), WhatsNewItemType.AllCoursesTopics,
            fixLocalUrls: false, domain, sideBar, cancellationToken);

        await CreateItemsListContentAsync(epub, title: "نویسندگان",
            page => docsInfoService.GetAuthorsTocPath(domain, page), itemsPerPage: 10,
            (page, itemsPerPage)
                => ePubExportDataProviderService.GetAuthorsAsync(page, itemsPerPage, cancellationToken),
            pagerPage => docsInfoService.GetAuthorsTocPath(domain, pagerPage), WhatsNewItemType.Posts,
            fixLocalUrls: false, domain, sideBar, cancellationToken);

        await CreateItemsListContentAsync(epub, title: "گروه‌ها", page => docsInfoService.GetTagsTocPath(domain, page),
            itemsPerPage: 10,
            (page, itemsPerPage)
                => ePubExportDataProviderService.GetArticleGroupsAsync(page, itemsPerPage, cancellationToken),
            pagerPage => docsInfoService.GetTagsTocPath(domain, pagerPage), WhatsNewItemType.Posts, fixLocalUrls: false,
            domain, sideBar, cancellationToken);

        await CreateItemsListContentAsync(epub, title: "نقشه‌های راه",
            page => docsInfoService.GetLearningPathsTocPath(domain, page), itemsPerPage: 10,
            (page, itemsPerPage)
                => ePubExportDataProviderService.GetLearningPathsAsync(page, itemsPerPage, cancellationToken),
            pagerPage => docsInfoService.GetLearningPathsTocPath(domain, pagerPage), WhatsNewItemType.LearningPaths,
            fixLocalUrls: true, domain, sideBar, cancellationToken);

        await CreateItemsListContentAsync(epub, title: "اشتراک‌ها",
            page => docsInfoService.GetNewsTocPath(domain, page), itemsPerPage: 100,
            (page, itemsPerPage) => ePubExportDataProviderService.GetNewsAsync(page, itemsPerPage, cancellationToken),
            pagerPage => docsInfoService.GetNewsTocPath(domain, pagerPage), WhatsNewItemType.News, fixLocalUrls: false,
            domain, sideBar, cancellationToken);
    }

    private void AddStaticAssets(EPubDocument epub)
    {
        foreach (var filePath in Directory.GetFiles(appFoldersService.FontsFolderPath, searchPattern: "*.*"))
        {
            epub.AddData($"fonts/{filePath.GetFileName()}", File.ReadAllBytes(filePath),
                mediaType: "application/x-font-ttf");
        }

        epub.AddStylesheetData(epubPath: "styles.css",
            File.ReadAllText(appFoldersService.ExportsAssetsFolder.SafePathCombine("styles.css"))
                .Replace(oldValue: "../", newValue: "", StringComparison.OrdinalIgnoreCase)
                .Replace(oldValue: "font-size: 14px;", newValue: "font-size: inherit;",
                    StringComparison.OrdinalIgnoreCase)
                .Replace(oldValue: "font-size: 10pt;", newValue: "font-size: inherit;",
                    StringComparison.OrdinalIgnoreCase));

        epub.AddStylesheetData(epubPath: "bootstrap.rtl.min.css",
            File.ReadAllText(appFoldersService.ExportsAssetsFolder.SafePathCombine("bootstrap.rtl.min.css")));

        epub.AddStylesheetData(epubPath: "vs.min.css",
            File.ReadAllText(appFoldersService.ExportsAssetsFolder.SafePathCombine("vs.min.css")));

        epub.AddData(epubPath: "SyntaxHighlighter.js",
            File.ReadAllBytes(appFoldersService.ExportsAssetsFolder.SafePathCombine("SyntaxHighlighter.js")),
            mediaType: "text/javascript");

        epub.AddData(epubPath: "highlight.min.js",
            File.ReadAllBytes(appFoldersService.ExportsAssetsFolder.SafePathCombine("highlight.min.js")),
            mediaType: "text/javascript");
    }

    private async Task AddContentsAsync(EPubDocument epub,
        WhatsNewItemType type,
        bool processSubItems,
        string domain,
        string sideBar,
        CancellationToken cancellationToken)
    {
        IList<EPubListItem>? posts = null;

        if (type == WhatsNewItemType.Posts)
        {
            posts = await ePubExportDataProviderService.GetAllArticlesAsync(cancellationToken);
        }
        else if (type == WhatsNewItemType.AllCoursesTopics)
        {
            _allCourses ??= await ePubExportDataProviderService.GetAllCoursesAsync(cancellationToken);
            posts = _allCourses;
        }
        else if (type == WhatsNewItemType.News)
        {
            posts = await ePubExportDataProviderService.GetAllNewsAsync(cancellationToken);
        }

        if (posts is null)
        {
            return;
        }

        foreach (var post in posts)
        {
            if (processSubItems)
            {
                if (post.SubItems is null)
                {
                    continue;
                }

                foreach (var item in post.SubItems)
                {
                    await AddContentItemAsync(epub, type, item, domain, sideBar, cancellationToken);
                }
            }
            else
            {
                await AddContentItemAsync(epub, type, post.Item, domain, sideBar, cancellationToken);
            }
        }
    }

    private async Task AddContentItemAsync(EPubDocument epub,
        WhatsNewItemType type,
        EPubContentItem item,
        string domain,
        string sideBar,
        CancellationToken cancellationToken)
    {
        var (content, title, fileName) = await docsInfoService.GetDocInfoAsync(type, item.Id, cancellationToken);
        var bodyNode = content.GetHtmlNodesByName(nodeName: "body").FirstOrDefault();

        if (bodyNode is null || title is null || fileName is null)
        {
            return;
        }

        var bodyHtml = await htmlProviderService.GetLastAndNextLinksHtmlAsync(type, item, cancellationToken) +
                       bodyNode.InnerHtml;

        content = htmlProviderService.ApplyHtmlPageTemplate(title, bodyHtml, sideBar);
        content = await FixEPubLocalUrlsAsync(content, domain, cancellationToken);
        epub.AddXhtmlData(fileName, content);
    }

    private async Task<string> FixEPubLocalUrlsAsync(string content, string domain, CancellationToken cancellationToken)
    {
        _allCourses ??= await ePubExportDataProviderService.GetAllCoursesAsync(cancellationToken);

        return await content.ReplaceAnchorUrlsWithNewUrlsAsync(async (pageUrl, _) =>
        {
            if (!pageUrl.IsValidUrl())
            {
                return null;
            }

            var uri = new Uri(pageUrl);

            if (!uri.Host.Contains(domain, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var postIds = learningPathPdfExportsService.GetPostIds([pageUrl]);

            if (postIds.Count > 0)
            {
                return (await docsInfoService.GetDocPathAsync(WhatsNewItemType.Posts, postIds[index: 0])).FileName;
            }

            var courseTopicIds = await learningPathPdfExportsService.GetCourseTopicIdsAsync([pageUrl]);

            if (courseTopicIds.Count > 0)
            {
                var topic = _allCourses.Where(ePubListItem => ePubListItem.SubItems is not null)
                    .SelectMany(ePubListItem => ePubListItem.SubItems!)
                    .FirstOrDefault(ePubContentItem => ePubContentItem.DisplayId == courseTopicIds[index: 0]);

                return topic is null
                    ? null
                    : (await docsInfoService.GetDocPathAsync(WhatsNewItemType.AllCoursesTopics, topic.Id)).FileName;
            }

            var newsIds = learningPathPdfExportsService.GetNewsIds([pageUrl]);

            if (newsIds.Count > 0)
            {
                return (await docsInfoService.GetDocPathAsync(WhatsNewItemType.News, newsIds[index: 0])).FileName;
            }

            return null;
        }, cancellationToken: cancellationToken);
    }

    private string CreateMainToc(EPubDocument epub, EPubTocItems items, string baseUrl, string domain)
    {
        var html = new StringBuilder();

        html.AppendLine(HtmlExtensions.CreateHtmlTable(
            $"<div class='row mt-3'><a target='_blank' style='text-align: center !important;' dir='ltr' href='{baseUrl}'>{domain.ToLowerInvariant()}</a></div>",
            [],
            [
                [
                    $"<a href='{docsInfoService.GetArticlesTocPath(domain, page: 1)}'>مطالب</a>",
                    htmlProviderService.WrapInBadge(items.ArticlesCount.ToPersianNumbers())
                ],
                [
                    $"<a href='{docsInfoService.GetAuthorsTocPath(domain, page: 1)}'>نویسندگان</a>",
                    htmlProviderService.WrapInBadge(items.AuthorsCount.ToPersianNumbers())
                ],
                [
                    $"<a href='{docsInfoService.GetTagsTocPath(domain, page: 1)}'>گروه‌های مطالب</a>",
                    htmlProviderService.WrapInBadge(items.ArticleGroupsCount.ToPersianNumbers())
                ],
                [
                    $"<a href='{docsInfoService.GetLearningPathsTocPath(domain, page: 1)}'>نقشه‌های راه</a>",
                    htmlProviderService.WrapInBadge(items.LearningPathsCount.ToPersianNumbers())
                ],
                [
                    $"<a href='{docsInfoService.GetCoursesTocPath(domain, page: 1)}'>دوره‌ها</a>",
                    htmlProviderService.WrapInBadge(items.CoursesCount.ToPersianNumbers())
                ],
                [
                    $"<a href='{docsInfoService.GetNewsTocPath(domain, page: 1)}'>اشتراک‌ها</a>",
                    htmlProviderService.WrapInBadge(items.NewsCount.ToPersianNumbers())
                ]
            ], tableClass: "table shadow-sm rounded table-hover mx-auto w-auto caption-top"));

        var content = htmlProviderService.ApplyHtmlPageTemplate(domain, html.ToString(), sideBar: null);
        epub.AddXhtmlData(docsInfoService.GetEPubTocPath(domain, page: 1), content);
        epub.AddNavPoint(domain, docsInfoService.GetEPubTocPath(domain, page: 1), playOrder: 0);

        return content;
    }

    private async Task CreateItemsListContentAsync(EPubDocument epub,
        string title,
        Func<int, string> navPointPath,
        int itemsPerPage,
        Func<int, int, Task<PagedResultModel<EPubListItem>>> getPageData,
        Func<int, string> getPageHref,
        WhatsNewItemType type,
        bool fixLocalUrls,
        string domain,
        string sideBar,
        CancellationToken cancellationToken)
    {
        var currentPage = 1;

        while (true)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(value: 100), cancellationToken);

            var pagedData = await getPageData(currentPage - 1, itemsPerPage);
            var totalItemCount = pagedData.TotalItems;
            var items = pagedData.Data;
            var numberOfPages = (int)Math.Ceiling(totalItemCount / (double)itemsPerPage);

            if (items.IsNullOrEmpty())
            {
                break;
            }

            var html = new StringBuilder();
            var listTitle = numberOfPages > 1 ? $"{title}، صفحه {currentPage.ToPersianNumbers()}" : title;
            htmlProviderService.AddHeader(html, listTitle);

            html.AppendLine(value: "<div class='mb-3'>");
            html.AppendLine(value: "<ul class='list-group'>");

            foreach (var listItem in items)
            {
                if (!listItem.SubItems.IsNullOrEmpty())
                {
                    htmlProviderService.AddHeader(html, listItem.Item.Title);

                    foreach (var subItem in listItem.SubItems.OrderBy(ePubContentItem => ePubContentItem.Id))
                    {
                        html.Append(await htmlProviderService.GetEPubContentItemLinkAsync(type, subItem));
                    }
                }
                else if (fixLocalUrls)
                {
                    htmlProviderService.AddHeader(html, listItem.Item.Title);

                    var itemContent =
                        await FixEPubLocalUrlsAsync(listItem.Item.Content ?? "", domain, cancellationToken);

                    html.AppendFormat(CultureInfo.InvariantCulture,
                        format: "<li class='list-group-item list-group-item-action'>{0}</li>", itemContent);
                }
                else
                {
                    html.Append(await htmlProviderService.GetEPubContentItemLinkAsync(type, listItem.Item));
                }
            }

            html.AppendLine(value: "</ul>");
            html.AppendLine(value: "</div>");

            if (numberOfPages > 1)
            {
                html.AppendLine(HtmlPaginator.CreateSimplePaginator(totalItemCount, itemsPerPage, currentPage,
                    showNumbersInPersian: true, getPageHref));
            }

            var pointPath = navPointPath(currentPage);

            epub.AddXhtmlData(pointPath,
                htmlProviderService.ApplyHtmlPageTemplate(listTitle, html.ToString(), sideBar));

            currentPage++;

            if (currentPage > numberOfPages)
            {
                break;
            }
        }

        await Task.Delay(TimeSpan.FromMilliseconds(value: 100), cancellationToken);
    }

    private static void AddMetaData(EPubDocument epub, string domain)
    {
        epub.AddBookIdentifier(domain);
        epub.AddLanguage(lang: "fa");
        epub.AddTitle(domain);
        epub.AddAuthor(domain);
        epub.AddDCItem(name: "contributor", domain);
        epub.AddDCItem(name: "publisher", domain);
        epub.AddDCItem(name: "subject", value: "Programming");

        epub.AddDCItem(name: "date",
            DateTimeOffset.UtcNow.ToString(format: "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));
    }
}
