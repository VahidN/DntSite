using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Common.Utils.WebToolkit;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.Components;

public partial class NewsArchive
{
    private const int ItemsPerPage = 10;

    private string? _basePath;
    private PagedResultModel<DailyNewsItem>? _posts;

    [InjectComponentScoped] internal IDailyNewsItemsService DailyNewsItemsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [Parameter] public string? Filter { set; get; }

    [Parameter] public int? RedirectId { set; get; }

    protected override async Task OnInitializedAsync()
    {
        if (RedirectId.HasValue)
        {
            await RedirectToOriginalUrlAsync(RedirectId.Value);
        }
        else
        {
            await ShowDailyNewsItemsAsync(Filter);
            AddBreadCrumbs();
        }
    }

    private async Task RedirectToOriginalUrlAsync(int redirectId)
    {
        if (ApplicationState.HttpContext.IsPostRequest())
        {
            return;
        }

        var newsItem = await DailyNewsItemsService.FindDailyNewsItemAsync(redirectId);

        if (newsItem is null || newsItem.IsDeleted)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        await DailyNewsItemsService.UpdateStatAsync(redirectId, ApplicationState.NavigationManager.IsFromFeed());

        ApplicationState.NavigateTo(newsItem.Url);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..NewsBreadCrumbs.DefaultBreadCrumbs]);

    private async Task DoSearchAsync(string gridifyFilter)
    {
        await ShowDailyNewsItemsAsync(gridifyFilter);
        StateHasChanged();
    }

    private async Task ShowDailyNewsItemsAsync(string? gridifyFilter)
    {
        CurrentPage ??= 1;

        _basePath = $"{NewsRoutingConstants.NewsFilterBase}/{Uri.EscapeDataString(gridifyFilter ?? "*")}";

        _posts = await DailyNewsItemsService.GetLastPagedDailyNewsItemsIncludeUserAndTagsAsync(new DntQueryBuilderModel
        {
            GridifyFilter = gridifyFilter.NormalizeGridifyFilter(),
            IsAscending = false,
            Page = CurrentPage.Value,
            PageSize = ItemsPerPage,
            SortBy = nameof(DailyNewsItem.Id)
        });
    }
}
