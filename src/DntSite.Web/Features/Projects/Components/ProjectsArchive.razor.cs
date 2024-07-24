using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectsArchive
{
    private const int ItemsPerPage = 5;

    private string? _basePath;
    private PagedResultModel<Project>? _posts;

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ShowProjectsAsync(Filter);
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..ProjectsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await ShowProjectsAsync(gridifyFilter);
        StateHasChanged();
    }

    private async Task ShowProjectsAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{ProjectsRoutingConstants.ProjectsFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await ProjectsService.GetLastPagedProjectsAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(Project.Id)
        });
    }
}
