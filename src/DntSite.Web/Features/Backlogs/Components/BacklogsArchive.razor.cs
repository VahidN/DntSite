using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.Backlogs.Components;

public partial class BacklogsArchive
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<Backlog>? _posts;

    [InjectComponentScoped] internal IBacklogsService BacklogsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ShowBacklogsAsync(Filter);
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..BacklogsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await ShowBacklogsAsync(gridifyFilter);
        StateHasChanged();
    }

    private async Task ShowBacklogsAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{BacklogsRoutingConstants.BacklogsFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await BacklogsService.GetLastPagedBacklogsAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(Backlog.Id)
        });
    }
}
