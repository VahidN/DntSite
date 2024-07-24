using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.StackExchangeQuestions.Entities;
using DntSite.Web.Features.StackExchangeQuestions.RoutingConstants;
using DntSite.Web.Features.StackExchangeQuestions.Services.Contracts;

namespace DntSite.Web.Features.StackExchangeQuestions.Components;

public partial class QuestionsTag
{
    private const int PostItemsPerPage = 10;
    private const int TagItemsPerPage = 30;
    private const string MainPageTitle = "گروه‌های پرسش‌ها";
    private const string MainPageUrl = QuestionsRoutingConstants.QuestionsTag;

    private PagedResultModel<StackExchangeQuestion>? _posts;
    private IList<(string Name, int InUseCount)>? _tags;
    private int _totalTagItemsCount;

    [MemberNotNullWhen(returnValue: true, nameof(TagName))]
    private bool HasTag => !string.IsNullOrWhiteSpace(TagName);

    private string MainTitle => !HasTag ? MainPageTitle : MainTagPageTitle;

    private string MainTagPageTitle => $@"آرشیو گروه‌های پرسش‌های {TagName}";

    private string MainTagPageUrl => !HasTag ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(TagName)}";

    private string BasePath => !HasTag ? MainPageUrl : MainTagPageUrl;

    [Parameter] public string? TagName { set; get; }

    [InjectComponentScoped] internal IQuestionsService QuestionsService { set; get; } = null!;

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (HasTag)
        {
            await ShowTagPostsAsync();
        }
        else
        {
            await ShowTagsListAsync();
        }
    }

    private async Task ShowTagsListAsync()
    {
        CurrentPage ??= 1;

        var results = await TagsService.GetPagedAllStackExchangeQuestionTagsListAsNoTrackingAsync(CurrentPage.Value - 1,
            TagItemsPerPage);

        _tags = results.Data.Select(x => (x.Name, x.InUseCount)).ToList();
        _totalTagItemsCount = results.TotalItems;

        AddTagsListBreadCrumbs();
    }

    private void AddTagsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..QuestionsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiTag
            }
        ]);

    private async Task ShowTagPostsAsync()
    {
        CurrentPage ??= 1;

        _posts = await QuestionsService.GetStackExchangeQuestionsByTagNameAsync(TagName!, CurrentPage.Value - 1,
            PostItemsPerPage);

        AddTagPostsBreadCrumbs();
    }

    private void AddTagPostsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..QuestionsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainPageTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiTag
            },
            new BreadCrumb
            {
                Title = MainTagPageTitle,
                Url = MainTagPageUrl,
                GlyphIcon = DntBootstrapIcons.BiTag
            }
        ]);
}
