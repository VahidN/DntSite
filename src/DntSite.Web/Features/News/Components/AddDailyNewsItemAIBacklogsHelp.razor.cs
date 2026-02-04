using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.News.RoutingConstants;

namespace DntSite.Web.Features.News.Components;

public partial class AddDailyNewsItemAIBacklogsHelp
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private string WritePostUrl
        => ApplicationState.RootUrl.CombineUrl(NewsRoutingConstants.AddDailyNewsItemAIBacklogs,
            escapeRelativeUrl: false);

    private string SendLinkUrl
        => $"""javascript:location.href="{WritePostUrl}?url="+encodeURIComponent(location.href)+"&title="+encodeURIComponent(document.title)""";
}
