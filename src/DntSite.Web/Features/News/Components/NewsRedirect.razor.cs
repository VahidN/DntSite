using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.News.Services.Contracts;

namespace DntSite.Web.Features.News.Components;

public partial class NewsRedirect
{
    private string? _pageTitle;

    [Parameter] public int? RedirectId { set; get; }

    [InjectComponentScoped] internal IDailyNewsItemsService DailyNewsItemsService { set; get; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override Task OnInitializedAsync() => RedirectToOriginalUrlAsync(RedirectId ?? 0);

    private async Task RedirectToOriginalUrlAsync(int redirectId)
    {
        var newsItem = await DailyNewsItemsService.FindDailyNewsItemAsync(redirectId);

        if (newsItem?.IsDeleted != false)
        {
            ApplicationState.NavigateToNotFoundPage();

            return;
        }

        _pageTitle = $"هدایت به «{newsItem.Title}»";
        await DailyNewsItemsService.UpdateStatAsync(redirectId, ApplicationState.NavigationManager.IsFromFeed());
        ApplicationState.NavigateTo(newsItem.Url);
    }
}
