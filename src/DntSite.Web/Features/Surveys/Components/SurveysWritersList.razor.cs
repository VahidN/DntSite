using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.RoutingConstants;
using DntSite.Web.Features.Surveys.Services.Contracts;
using DntSite.Web.Features.UserProfiles.Entities;
using DntSite.Web.Features.UserProfiles.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Components;

public partial class SurveysWritersList
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نویسنده‌های نظرسنجی‌ها";
    private const string MainPageUrl = SurveysRoutingConstants.SurveysWriters;

    private PagedResultModel<Survey>? _blogPosts;
    private int _totalUsersCount;
    private IList<(User User, string NumberOfPosts)>? _users;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => $@"آرشیو نظرسنجی‌های {UserFriendlyName}";

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IVotesService SurveysService { set; get; } = null!;

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

        var results = await UsersService.GetPagedSurveyWritersListAsync(CurrentPage.Value - 1, PostItemsPerPage);

        _users = results.Data
            .Select(user => (user, user.UserStat.NumberOfSurveys.ToString(CultureInfo.InvariantCulture)))
            .ToList();

        _totalUsersCount = results.TotalItems;

        AddUsersListBreadCrumbs();
    }

    private void AddUsersListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..SurveysBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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
            await SurveysService.GetLastVotesByUserAsync(UserFriendlyName!, CurrentPage.Value - 1, PostItemsPerPage);

        AddUserPostsBreadCrumbs();
    }

    private void AddUserPostsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..SurveysBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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
