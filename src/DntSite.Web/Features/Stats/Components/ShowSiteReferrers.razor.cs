using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Stats.Entities;
using DntSite.Web.Features.Stats.Models;
using DntSite.Web.Features.Stats.RoutingConstants;
using DntSite.Web.Features.Stats.Services.Contracts;

namespace DntSite.Web.Features.Stats.Components;

public partial class ShowSiteReferrers
{
    private const int ItemsPerPage = 10;

    private PagedResultModel<SiteReferrer>? _items;

    private SiteReferrerType _siteReferrerType = SiteReferrerType.External;

    [InjectComponentScoped] internal ISiteReferrersService SiteReferrersService { set; get; } = null!;

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? DeleteId { set; get; }

    [Parameter] public string? ReferrerType { set; get; }

    private string BasePath => $"{StatsRoutingConstants.SiteReferrersBase}/{_siteReferrerType}";

    private string PageTitle => string.Create(CultureInfo.InvariantCulture,
            $"ارجاع دهنده‌های {(_siteReferrerType == SiteReferrerType.External ? "خارجی" : "داخلی")}، صفحه {CurrentPage ?? 1}")
        .ToPersianNumbers();

    protected override async Task OnInitializedAsync()
    {
        SetSiteReferrerType();
        await TryDeleteItemAsync();
        await ShowResultsAsync();
        AddBreadCrumbs();
    }

    private async Task TryDeleteItemAsync()
    {
        if (!string.IsNullOrWhiteSpace(DeleteId))
        {
            await SiteReferrersService.RemoveSiteReferrerAsync(DeleteId.ToInt());
        }
    }

    private void SetSiteReferrerType()
    {
        if (string.IsNullOrWhiteSpace(ReferrerType))
        {
            _siteReferrerType = SiteReferrerType.External;
        }
        else
        {
            Enum.TryParse(ReferrerType, ignoreCase: true, out _siteReferrerType);
        }
    }

    private async Task ShowResultsAsync()
    {
        CurrentPage ??= 1;

        _items = await SiteReferrersService.GetPagedSiteReferrersAsync(CurrentPage.Value - 1, ItemsPerPage,
            _siteReferrerType == SiteReferrerType.Internal);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..StatsBreadCrumbs.SiteStatsBreadCrumbs]);
}
