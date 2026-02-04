using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.Models;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace DntSite.Web.Features.News.Components;

[Authorize(Roles = CustomRoles.Admin)]
public partial class AddDailyNewsItemAIBacklogs
{
    private List<DailyNewsItemAIBacklog>? _backlogs;

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    [SupplyParameterFromForm] public string? Urls { set; get; }

    [SupplyParameterFromForm] public AIBacklogAction BacklogActionValue { set; get; }

    [SupplyParameterFromForm] public IList<int>? SelectedDeleteIds { set; get; }

    [SupplyParameterFromForm] public IList<int>? SelectedApproveIds { set; get; }

    [InjectComponentScoped] internal IDailyNewsItemAIBacklogService DailyNewsItemAIBacklogService { set; get; } = null!;

    private async Task PerformAsync()
    {
        switch (BacklogActionValue)
        {
            case AIBacklogAction.AddUrls:
                await DailyNewsItemAIBacklogService.AddDailyNewsItemAIBacklogsAsync(Urls,
                    ApplicationState.CurrentUser?.User);

                Urls = string.Empty;

                break;
            case AIBacklogAction.ApplyChanges:
                var recordIds = await DailyNewsItemAIBacklogService.GetNotProcessedDailyNewsItemAIBacklogIdsAsync();

                await DailyNewsItemAIBacklogService.MarkAsDeletedOrApprovedAsync(recordIds, SelectedDeleteIds,
                    SelectedApproveIds);

                break;
        }

        await ShowDataAsync();
    }

    private async Task ShowDataAsync()
        => _backlogs = await DailyNewsItemAIBacklogService.GetNotProcessedDailyNewsItemAIBacklogsAsync();

    protected override async Task OnInitializedAsync()
    {
        AddBreadCrumbs();

        if (ApplicationState.HttpContext.IsGetRequest())
        {
            FillPossibleFormItemsFromUrl();
            await ShowDataAsync();
        }
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([NewsBreadCrumbs.AddAINewsBacklog]);

    private void FillPossibleFormItemsFromUrl()
    {
        var uri = ApplicationState.NavigationManager.ToAbsoluteUri(ApplicationState.NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);

        if (query.TryGetValue(key: "url", out var url) && query.TryGetValue(key: "title", out var title))
        {
            Urls = $"{url}{NewsRoutingConstants.UrlTitleSeparator}{title}";
        }
    }
}
