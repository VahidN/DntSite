using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;

namespace DntSite.Web.Features.News.Components;

public partial class PrintNewsRedirect
{
    [Parameter] [EditorRequired] public DailyNewsItem? DailyNewsItem { set; get; }

    private string StatusIcon => DailyNewsItem?.LastHttpStatusCode is HttpStatusCode.OK ? "bg-success" : "bg-warning";

    private string HttpStatusCodeText => DailyNewsItem?.LastHttpStatusCode is null
        ? "نامشخص"
        : string.Create(CultureInfo.InvariantCulture,
            $"{(int)DailyNewsItem.LastHttpStatusCode.Value}, {DailyNewsItem.LastHttpStatusCode.Value}");

    private string UrlHost => DailyNewsItem is not null ? new Uri(DailyNewsItem.Url).Host : "";

    private string GetUrl(DailyNewsItem dailyNewsItem)
        => string.Create(CultureInfo.InvariantCulture, $"{NewsRoutingConstants.NewsRedirectBase}/{dailyNewsItem.Id}");
}
