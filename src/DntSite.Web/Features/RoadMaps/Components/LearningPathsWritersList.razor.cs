using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.RoadMaps.Entities;
using DntSite.Web.Features.RoadMaps.RoutingConstants;
using DntSite.Web.Features.RoadMaps.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.RoadMaps.Components;

public partial class LearningPathsWritersList
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نویسنده‌های نقشه‌های راه";
    private const string MainPageUrl = RoadMapsRoutingConstants.LearningPathsWriters;

    private PagedResultModel<LearningPath>? _blogPosts;
    private int _totalUsersCount;
    private IList<(User User, string NumberOfPosts)>? _users;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => $@"آرشیو نقشه‌های راه {UserFriendlyName}";

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal ILearningPathService LearningPathService { set; get; } = null!;

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

        var results = await UsersService.GetPagedLearningPathWritersListAsync(CurrentPage.Value - 1, PostItemsPerPage);

        _users = results.Data
            .Select(user => (user, user.UserStat.NumberOfLearningPaths.ToString(CultureInfo.InvariantCulture)))
            .ToList();

        _totalUsersCount = results.TotalItems;

        AddUsersListBreadCrumbs();
    }

    private void AddUsersListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..RoadMapsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private async Task ShowUserPostsAsync()
    {
        CurrentPage ??= 1;

        var showAll = string.Equals(ApplicationState.CurrentUser?.User?.FriendlyName, UserFriendlyName,
            StringComparison.OrdinalIgnoreCase);

        _blogPosts = await LearningPathService.GetLastPagedLearningPathsOfUserAsync(UserFriendlyName!,
            CurrentPage.Value - 1, PostItemsPerPage, showAll: showAll);

        AddUserPostsBreadCrumbs();
    }

    private void AddUserPostsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..RoadMapsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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
