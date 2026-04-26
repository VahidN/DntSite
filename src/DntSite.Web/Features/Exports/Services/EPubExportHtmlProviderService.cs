using System.Text;
using DntSite.Web.Features.Exports.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.Exports.Services;

public class EPubExportHtmlProviderService(
    IPdfExportService pdfExportService,
    IEPubExportDocsInfoService docsInfoService) : IEPubExportHtmlProviderService
{
    public string ApplyHtmlPageTemplate(string title, string body, string? sideBar)
    {
        var content = string.Format(CultureInfo.InvariantCulture, pdfExportService.GetPageTemplateContent(),
            title.ApplyRle(), body);

        return sideBar is null
            ? content
            : $"""
               <div class='container-fluid min-vh-100 d-flex flex-column'>
                   <div class='row flex-grow-1'>
                       <div class='col-md-2'>
                        {sideBar}
                       </div>
                       <div class='col-md-10'>
                           <div class='mt-4 mb-3 container-fluid'>
                             {content}
                           </div>
                       </div>
                   </div>
               </div>
               """;
    }

    public async Task<string> GetEPubContentItemLinkAsync(WhatsNewItemType type, EPubContentItem subItem)
    {
        ArgumentNullException.ThrowIfNull(subItem);

        var dateBadge = subItem.PublishDate.HasValue
            ? string.Format(CultureInfo.InvariantCulture,
                format: "<span class='ms-2 badge bg-secondary rounded-pill' dir='ltr'>{0}</span>",
                subItem.PublishDate.ToShortPersianDateString().ToPersianNumbers())
            : "";

        var authorBadge = !subItem.Author.IsEmpty()
            ? string.Format(CultureInfo.InvariantCulture,
                format: "<span class='ms-2 badge bg-primary rounded-pill' dir='rtl'>{0}</span>", subItem.Author)
            : "";

        var urlBadge = !subItem.Url.IsEmpty()
            ? string.Format(CultureInfo.InvariantCulture,
                format:
                "<a class='ms-2 badge border-sm bg-light rounded-pill' dir='rtl' target='_blank' href='{0}'>مشاهده</a>",
                subItem.Url)
            : "";

        return string.Format(CultureInfo.InvariantCulture,
            format:
            "<li class='list-group-item list-group-item-action'><a dir='rtl' href='{0}'>{1}</a> {2} {3} {4}</li>",
            (await docsInfoService.GetDocPathAsync(type, subItem.Id)).FileName, subItem.Title, authorBadge, dateBadge,
            urlBadge);
    }

    public void AddHeader(StringBuilder html, string title)
    {
        ArgumentNullException.ThrowIfNull(html);
        html.AppendLine(value: "<div class='card mt-2 mb-2' align='center'>");

        html.AppendLine(title.ContainsFarsi()
            ? $"<h2 class='card-header shadow-sm align-items-center'>{title}</h2>"
            : $"<h2 class='card-header shadow-sm align-items-center' dir='ltr'>{title}</h2>");

        html.AppendLine(value: "</div>");
    }

    public string WrapInBadge(string title)
        => string.Create(CultureInfo.InvariantCulture, $"<span class='badge bg-secondary rounded-pill'>{title}</span>");

    public async Task<string> GetLastAndNextLinksHtmlAsync(WhatsNewItemType type,
        EPubContentItem item,
        CancellationToken cancellationToken)
    {
        var (nextPostTitle, nextPostItemUrl) =
            await docsInfoService.GetNextLastItemsAsync(type, item, isAscending: true, numberOfTries: 30,
                cancellationToken);

        var (lastPostTitle, lastPostItemUrl) = await docsInfoService.GetNextLastItemsAsync(type, item,
            isAscending: false, numberOfTries: 30, cancellationToken);

        var html = new StringBuilder();

        html.Append(
            value:
            "<nav class='card shadow-sm d-flex flex-row flex-wrap justify-content-between align-items-center pe-3 ps-3 pt-1 pb-2 mb-4' dir='rtl'>");

        if (!string.IsNullOrWhiteSpace(nextPostTitle))
        {
            html.Append(CultureInfo.InvariantCulture, $"""
                                                       <a href='{nextPostItemUrl}' class='link-offset-2 link-offset-3-hover link-underline link-underline-opacity-0 link-underline-opacity-75-hover'>
                                                          <h6 class='mt-3'>
                                                              <span aria-hidden='true'>&rarr;</span>
                                                              <span dir='{nextPostTitle.GetDir()}'>{nextPostTitle}</span>
                                                          </h6>
                                                       </a>
                                                       """);
        }
        else
        {
            html.Append(value: "<div></div>");
        }

        if (!string.IsNullOrWhiteSpace(lastPostTitle))
        {
            html.Append(CultureInfo.InvariantCulture, $"""
                                                       <a href='{lastPostItemUrl}' class='link-offset-2 link-offset-3-hover link-underline link-underline-opacity-0 link-underline-opacity-75-hover'>
                                                           <h6 class='mt-3'>
                                                               <span dir='{lastPostTitle.GetDir()}'>{lastPostTitle}</span>
                                                               <span aria-hidden='true'>&larr;</span>
                                                           </h6>
                                                       </a>
                                                       """);
        }
        else
        {
            html.Append(value: "<div></div>");
        }

        html.Append(value: "</nav>");

        return html.ToString();
    }
}
