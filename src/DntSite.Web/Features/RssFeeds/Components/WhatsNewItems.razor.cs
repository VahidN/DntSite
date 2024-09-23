using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;

namespace DntSite.Web.Features.RssFeeds.Components;

public partial class WhatsNewItems
{
    [Parameter] public PagedResultModel<LuceneSearchResult>? Posts { set; get; }

    [Parameter] public int ItemsPerPage { set; get; }

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public bool ShowPager { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }
}
