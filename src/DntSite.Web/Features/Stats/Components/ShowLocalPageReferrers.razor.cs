using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Components;

public partial class ShowLocalPageReferrers
{
    private PagedResultModel<SiteReferrer>? _rows;

    [Parameter] public string? LocalUrl { set; get; }

    [Parameter] public int PageNumber { set; get; } = 1;

    [Parameter] public int RecordsPerPage { set; get; } = 10;

    [Parameter] public bool ShowPager { set; get; }

    [InjectComponentScoped] internal ISiteReferrersService SiteReferrersService { set; get; } = null!;

    [CascadingParameter] public HttpContext HttpContext { get; set; } = null!;

    private string MoreLocalPageReferrersUrl => "/".CombineUrl(StatsRoutingConstants.MoreLocalPageReferrersBase)
        .CombineUrl(Uri.EscapeDataString(LocalUrl ?? ""));

    private string BasePath => MoreLocalPageReferrersUrl;

    private bool HasMoreData => _rows is not null && _rows.TotalItems > PageNumber * RecordsPerPage;

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(LocalUrl))
        {
            LocalUrl = HttpContext.GetRawUrl();
        }

        _rows = await SiteReferrersService.GetPagedSiteReferrersAsync(LocalUrl, PageNumber - 1, RecordsPerPage,
            isLocalReferrer: true);
    }
}
