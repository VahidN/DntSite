using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.Searches.Services.Contracts;

namespace DntSite.Web.Features.RoadMaps.Components;

public partial class LearningPathsArchive
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<LearningPath>? _posts;

    [InjectComponentScoped] internal ILearningPathService LearningPathService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await ShowLearningPathsAsync(Filter);
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..RoadMapsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await SearchItemsService.SaveSearchItemAsync(gridifyFilter);

        ApplicationState.NavigateTo(
            $"{RoadMapsRoutingConstants.LearningPathsFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}/page/1");
    }

    private async Task ShowLearningPathsAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{RoadMapsRoutingConstants.LearningPathsFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await LearningPathService.GetLastPagedLearningPathsAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(LearningPath.Id)
        });
    }
}
