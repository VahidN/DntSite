using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Bookmarks.Models;
using DntSite.Web.Features.Bookmarks.RoutingConstants;
using DntSite.Web.Features.Bookmarks.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.Bookmarks.Components;

[Authorize]
public partial class MyBookmarks
{
    private const int ItemsPerPage = 30;

    private const string MainTitle = "علاقمندی‌های من";

    private PagedResultModel<BookmarkDto>? _items;

    [InjectComponentScoped] internal IBookmarksService BookmarksService { get; set; } = null!;

    [CascadingParameter] internal ApplicationState ApplicationState { get; set; } = null!;

    [Parameter] public int? CurrentPage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await ShowResultsAsync();
        AddBreadCrumbs();
    }

    private async Task ShowResultsAsync()
    {
        var userId = ApplicationState.CurrentUser?.UserId;

        if (!userId.HasValue)
        {
            ApplicationState.NavigateToUnauthorizedPage();

            return;
        }

        CurrentPage ??= 1;

        _items = await BookmarksService.GetAllUserBookmarksAsync(userId.Value, CurrentPage.Value - 1, ItemsPerPage);
    }

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([BookmarksBreadCrumbs.MyBookmarks]);
}
