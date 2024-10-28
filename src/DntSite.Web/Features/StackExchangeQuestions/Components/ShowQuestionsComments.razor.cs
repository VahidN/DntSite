using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Components;

public partial class ShowQuestionsComments
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نظرات پرسش‌ها";
    private const string MainPageUrl = QuestionsRoutingConstants.QuestionsComments;

    private PagedResultModel<StackExchangeQuestionComment>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => string.Create(CultureInfo.InvariantCulture,
        $"آرشیو نظرات پرسش‌های {UserFriendlyName}، صفحه: {CurrentPage ?? 1}");

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IQuestionsCommentsService QuestionsCommentsService { set; get; } = null!;

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

        _posts = await QuestionsCommentsService.GetLastPagedStackExchangeQuestionCommentsAsNoTrackingAsync(
            CurrentPage.Value - 1, PostItemsPerPage);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..QuestionsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiPerson
            }
        ]);

    private async Task ShowUserCommentsAsync()
    {
        CurrentPage ??= 1;

        _posts = await QuestionsCommentsService.GetLastPagedStackExchangeQuestionCommentsOfUserAsync(UserFriendlyName!,
            CurrentPage.Value - 1, PostItemsPerPage);

        AddUserCommentsBreadCrumbs();
    }

    private void AddUserCommentsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..QuestionsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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

    private string GetPostAbsoluteUrl(StackExchangeQuestionComment comment)
        => string.Create(CultureInfo.InvariantCulture,
            $"{QuestionsRoutingConstants.QuestionsDetailsBase}/{comment.ParentId}");
}
