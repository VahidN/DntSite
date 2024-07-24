using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.News.RoutingConstants;

namespace DntSite.Web.Features.News.Components;

public partial class WriteNewsHelp
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private string RootUrl
        => ApplicationState.AppSetting?.SiteRootUri ?? ApplicationState.CurrentAbsoluteUri.ToString();

    private string WritePostUrl => RootUrl.CombineUrl(NewsRoutingConstants.DailyLinks);

    private string SendLinkUrl
        => $"""javascript:location.href="{WritePostUrl}?url="+encodeURIComponent(location.href)+"&title="+encodeURIComponent(document.title)""";
}
