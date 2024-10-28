using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;
using DntSite.Web.Features.Projects.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Projects.Components;

public partial class ProjectsWritersList
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نویسنده‌های پروژه‌ها";
    private const string MainPageUrl = ProjectsRoutingConstants.ProjectsWriters;

    private PagedResultModel<Project>? _blogPosts;
    private int _totalUsersCount;
    private IList<(User User, string NumberOfPosts)>? _users;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => string.Create(CultureInfo.InvariantCulture,
        $"آرشیو پروژه‌های {UserFriendlyName}، صفحه: {CurrentPage ?? 1}");

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IProjectsService ProjectsService { set; get; } = null!;

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

        var results = await UsersService.GetPagedProjectsWritersListAsync(CurrentPage.Value - 1, PostItemsPerPage);

        _users = results.Data
            .Select(user => (user, user.UserStat.NumberOfProjects.ToString(CultureInfo.InvariantCulture)))
            .ToList();

        _totalUsersCount = results.TotalItems;

        AddUsersListBreadCrumbs();
    }

    private void AddUsersListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..ProjectsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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
            await ProjectsService.GetLastProjectsByAuthorIncludeAuthorTagsAsync(UserFriendlyName!,
                CurrentPage.Value - 1, PostItemsPerPage);

        AddUserPostsBreadCrumbs();
    }

    private void AddUserPostsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..ProjectsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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
