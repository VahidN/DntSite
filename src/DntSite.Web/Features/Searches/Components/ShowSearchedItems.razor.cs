using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Models;
using DntSite.Web.Features.Searches.RoutingConstants;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Searches.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class ShowSearchedItems
{
    private const int ItemsPerPage = 20;

    private PagedResultModel<SearchItemModel>? _items;

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

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

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..SearchesBreadCrumbs.DefaultBreadCrumbs]);
}
