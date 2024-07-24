using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Components;

public partial class QuestionsArchive
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<StackExchangeQuestion>? _posts;

    [InjectComponentScoped] internal IQuestionsService QuestionsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    protected override async Task OnInitializedAsync()
    {
        await ShowDailyQuestionsItemsAsync(Filter);
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..QuestionsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await ShowDailyQuestionsItemsAsync(gridifyFilter);
        StateHasChanged();
    }

    private async Task ShowDailyQuestionsItemsAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{QuestionsRoutingConstants.QuestionsFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await QuestionsService.GetLastPagedStackExchangeQuestionsAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(StackExchangeQuestion.Id)
        });
    }
}
