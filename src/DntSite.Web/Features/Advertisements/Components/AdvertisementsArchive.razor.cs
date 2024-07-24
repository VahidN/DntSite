using DntSite.Web.Features.Advertisements.Entities;
using DntSite.Web.Features.Advertisements.RoutingConstants;
using DntSite.Web.Features.Advertisements.Services.Contracts;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.Advertisements.Components;

public partial class AdvertisementsArchive
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<Advertisement>? _posts;

    [InjectComponentScoped] internal IAdvertisementsService AdvertisementsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ShowAdvertisementsAsync(Filter);
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([..AdvertisementsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await ShowAdvertisementsAsync(gridifyFilter);
        StateHasChanged();
    }

    private async Task ShowAdvertisementsAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath =
            $"{AdvertisementsRoutingConstants.AdvertisementsFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await AdvertisementsService.GetLastPagedAdvertisementsAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(Advertisement.Id)
        });
    }
}
