using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.RoutingConstants;
using DntSite.Web.Features.Backlogs.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Backlogs.Components;

public partial class BacklogsWritersList
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نویسنده‌های پیشنهاد‌ها";
    private const string MainPageUrl = BacklogsRoutingConstants.BacklogsWriters;

    private PagedResultModel<Backlog>? _blogPosts;
    private int _totalUsersCount;
    private IList<(User User, int NumberOfPosts)>? _users;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => $@"آرشیو پیشنهادهای {UserFriendlyName}";

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IBacklogsService BacklogsService { set; get; } = null!;

    [InjectComponentScoped] internal IUsersInfoService UsersService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (HasUserFriendlyName)
        {
            await ShowUserPostsAsync();
        }
        else
        {
            await ShowUsersListAsync();
        }
    }

    private async Task ShowUsersListAsync()
    {
        CurrentPage ??= 1;

        var results = await UsersService.GetPagedBacklogsWritersListAsync(CurrentPage.Value - 1, PostItemsPerPage);

        _users = results.Data.Select(user => (user, user.UserStat.NumberOfBacklogs)).ToList();
        _totalUsersCount = results.TotalItems;

        AddUsersListBreadCrumbs();
    }

    private void AddUsersListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..BacklogsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private async Task ShowUserPostsAsync()
    {
        CurrentPage ??= 1;

        _blogPosts =
            await BacklogsService.GetLastPagedBacklogsAsync(UserFriendlyName!, CurrentPage.Value - 1, PostItemsPerPage);

        AddUserPostsBreadCrumbs();
    }

    private void AddUserPostsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..BacklogsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainPageTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            },
            new BreadCrumb
            {
                Title = MainUserPageTitle,
                Url = MainUserPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);
}
