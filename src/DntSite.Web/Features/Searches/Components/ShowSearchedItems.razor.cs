using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Stats.RoutingConstants;

namespace DntSite.Web.Features.Searches.Components;

public partial class ShowSearchedItems
{
    private const int ItemsPerPage = 20;

    private const string MainTitle = "آمار جستجوها";

    private PagedResultModel<SearchItemModel>? _items;

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    private bool IsCurrentUserAdmin => ApplicationState.CurrentUser?.IsAdmin == true;

    protected override async Task OnInitializedAsync()
    {
        await ShowResultsAsync();
        AddBreadCrumbs();
    }

    private async Task ShowResultsAsync()
    {
        CurrentPage ??= 1;

        _items = await SearchItemsService.GetPagedSearchItemsAsync(CurrentPage.Value - 1, ItemsPerPage);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..StatsBreadCrumbs.SiteStatsBreadCrumbs]);
}
