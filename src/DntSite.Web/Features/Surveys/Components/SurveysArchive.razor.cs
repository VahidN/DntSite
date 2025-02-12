using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Searches.Services.Contracts;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.RoutingConstants;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Components;

public partial class SurveysArchive
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<Survey>? _posts;

    [InjectComponentScoped] internal IVotesService VotesService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    [InjectComponentScoped] internal ISearchItemsService SearchItemsService { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();
        await ShowSurveysAsync(Filter);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..SurveysBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await SearchItemsService.SaveSearchItemAsync(gridifyFilter);

        ApplicationState.NavigateTo(
            $"{SurveysRoutingConstants.SurveysArchiveFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}/page/1");
    }

    private async Task ShowSurveysAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{SurveysRoutingConstants.SurveysArchiveFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await VotesService.GetLastPagedSurveysAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(Survey.Id)
        });
    }
}
