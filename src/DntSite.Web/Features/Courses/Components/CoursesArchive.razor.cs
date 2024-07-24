using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;
using DntSite.Web.Features.Courses.RoutingConstants;
using DntSite.Web.Features.Courses.Services.Contracts;

namespace DntSite.Web.Features.Courses.Components;

public partial class CoursesArchive
{
    private const int ItemsPerPage = 5;

    private string? _basePath;
    private PagedResultModel<Course>? _posts;

    [InjectComponentScoped] internal ICoursesService CoursesService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ShowCoursesAsync(Filter);
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..CoursesBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await ShowCoursesAsync(gridifyFilter);
        StateHasChanged();
    }

    private async Task ShowCoursesAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{CoursesRoutingConstants.CoursesFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await CoursesService.GetLastPagedCoursesAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(Course.Id)
        });
    }
}
