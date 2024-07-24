using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.RoutingConstants;
using DntSite.Web.Features.Surveys.Services.Contracts;

namespace DntSite.Web.Features.Surveys.Components;

public partial class ShowSurveysComments
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نظرات نظرسنجی‌ها";
    private const string MainPageUrl = SurveysRoutingConstants.SurveysComments;

    private PagedResultModel<SurveyComment>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => $@"آرشیو نظرات نظرسنجی‌های {UserFriendlyName}";

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IVoteCommentsService SurveyCommentsService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (HasUserFriendlyName)
        {
            await ShowUserCommentsAsync();
        }
        else
        {
            await ShowCommentsListAsync();
        }
    }

    private async Task ShowCommentsListAsync()
    {
        CurrentPage ??= 1;

        _posts = await SurveyCommentsService.GetPagedLastVoteCommentsAsync(CurrentPage.Value - 1);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..SurveysBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private async Task ShowUserCommentsAsync()
    {
        CurrentPage ??= 1;

        _posts = await SurveyCommentsService.GetLastPagedVotesCommentsAsNoTrackingAsync(UserFriendlyName!,
            CurrentPage.Value - 1, PostItemsPerPage);

        AddUserCommentsBreadCrumbs();
    }

    private void AddUserCommentsBreadCrumbs()
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

    private string GetPostAbsoluteUrl(SurveyComment comment)
        => Invariant($"{SurveysRoutingConstants.SurveysArchiveDetailsBase}/{comment.ParentId}");
}
