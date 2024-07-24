using AutoMapper;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using Microsoft.AspNetCore.WebUtilities;

namespace DntSite.Web.Features.News.Components;

[Authorize]
public partial class WriteNews
{
    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    public IList<string>? AutoCompleteDataList { get; set; }

    [SupplyParameterFromForm(FormName = nameof(WriteNews))]
    public DailyNewsItemModel WriteNewsModel { get; set; } = new();

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public int? EditId { set; get; }

    [Parameter] public int? DeleteId { set; get; }

    [InjectComponentScoped] internal IDailyNewsItemsService DailyNewsItemsService { set; get; } = null!;

    [Inject] internal IMapper Mapper { set; get; } = null!;

    [CascadingParameter] internal DntAlert Alert { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AutoCompleteDataList = await TagsService.GetTagNamesArrayAsync(count: 2000);
        AddBreadCrumbs();

        if (!ApplicationState.HttpContext.IsGetRequest())
        {
            return;
        }

        await PerformPossibleDeleteAsync();

        await FillPossibleEditFormAsync();
        FillPossibleEditFormFromUrl();
    }

    private async Task PerformPossibleDeleteAsync()
    {
        if (!DeleteId.HasValue)
        {
            return;
        }

        var newsItem = await GetUserDailyNewsItemAsync(DeleteId.Value);
        await DailyNewsItemsService.MarkAsDeletedAsync(newsItem);
        await DailyNewsItemsService.NotifyDeleteChangesAsync(newsItem, ApplicationState.CurrentUser?.User);

        ApplicationState.NavigateTo(NewsRoutingConstants.News);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..NewsBreadCrumbs.DefaultBreadCrumbs]);

    private void FillPossibleEditFormFromUrl()
    {
        var uri = ApplicationState.NavigationManager.ToAbsoluteUri(ApplicationState.NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);

        if (query.TryGetValue(key: "url", out var url) && query.TryGetValue(key: "title", out var title))
        {
            WriteNewsModel = new DailyNewsItemModel
            {
                Url = url.ToString(),
                Title = title.ToString()
            };
        }
    }

    private async Task FillPossibleEditFormAsync()
    {
        if (!EditId.HasValue)
        {
            return;
        }

        var item = await GetUserDailyNewsItemAsync(EditId.Value);

        if (item is null)
        {
            return;
        }

        WriteNewsModel = Mapper.Map<DailyNewsItem, DailyNewsItemModel>(item);
    }

    private async Task<DailyNewsItem?> GetUserDailyNewsItemAsync(int id)
    {
        var newsItem = await DailyNewsItemsService.GetDailyNewsItemAsync(id);

        if (newsItem is null || !ApplicationState.CanCurrentUserEditThisItem(newsItem.UserId, newsItem.Audit.CreatedAt))
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return null;
        }

        return newsItem;
    }

    private async Task PerformAsync()
    {
        var checkUrlHashResult = await DailyNewsItemsService.CheckUrlHashAsync(WriteNewsModel.Url, EditId,
            ApplicationState.CurrentUser?.IsAdmin == true);

        if (checkUrlHashResult.Stat == OperationStat.Failed)
        {
            Alert.ShowAlert(AlertType.Danger, title: "خطا!", checkUrlHashResult.Message);

            return;
        }

        var user = ApplicationState.CurrentUser?.User;

        DailyNewsItem? newsItem;

        if (EditId.HasValue)
        {
            newsItem = await GetUserDailyNewsItemAsync(EditId.Value);
            await DailyNewsItemsService.UpdateNewsItemAsync(newsItem, WriteNewsModel);
        }
        else
        {
            newsItem = await DailyNewsItemsService.AddNewsItemAsync(WriteNewsModel, user);
        }

        await DailyNewsItemsService.NotifyAddOrUpdateChangesAsync(newsItem, WriteNewsModel, user);

        ApplicationState.NavigateTo(Invariant($"{NewsRoutingConstants.NewsDetailsBase}/{newsItem?.Id}"));
    }
}
