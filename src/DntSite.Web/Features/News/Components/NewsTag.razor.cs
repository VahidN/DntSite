using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Exports.Services.Contracts;
using DntSite.Web.Features.News.Entities;
using DntSite.Web.Features.News.RoutingConstants;
using DntSite.Web.Features.News.Services.Contracts;
using DntSite.Web.Features.RssFeeds.Models;

namespace DntSite.Web.Features.News.Components;

public partial class NewsTag
{
    private const int PostItemsPerPage = 10;
    private const int TagItemsPerPage = 30;
    private const string MainPageTitle = "گروه‌های اشتراک‌ها";
    private const string MainPageUrl = NewsRoutingConstants.NewsTag;

    private bool _isExportedPdfFileReady;
    private PagedResultModel<DailyNewsItem>? _posts;
    private int? _tagId;
    private IList<(string Name, int Id, int InUseCount)>? _tags;
    private int _totalTagItemsCount;

    [MemberNotNullWhen(returnValue: true, nameof(TagName))]
    private bool HasTag => !string.IsNullOrWhiteSpace(TagName);

    private string MainTitle => !HasTag ? MainPageTitle : MainTagPageTitle;

    private string MainTagPageTitle => string.Create(CultureInfo.InvariantCulture,
        $"آرشیو گروه‌های اشتراک‌های {TagName}");

    private string MainTagPageUrl => !HasTag ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(TagName)}";

    private string BasePath => !HasTag ? MainPageUrl : MainTagPageUrl;

    [Parameter] public string? TagName { set; get; }

    [InjectComponentScoped] internal IDailyNewsItemsService DailyNewsItemsService { set; get; } = null!;

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

    [InjectComponentScoped] internal IPdfExportService PdfExportService { set; get; } = null!;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private string TagCaption => $"دریافت تمام مطالب گروه {TagName} با فرمت PDF";

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

        var results = await TagsService.GetPagedAllLinkTagsListAsNoTrackingAsync(CurrentPage.Value - 1,
            TagItemsPerPage);

        _tags = results.Data.Select(x => (x.Name, x.Id, x.InUseCount)).ToList();
        _totalTagItemsCount = results.TotalItems;

        AddTagsListBreadCrumbs();
    }

    private void AddTagsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..NewsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiTag
            }
        ]);

    private async Task ShowTagPostsAsync()
    {
        CurrentPage ??= 1;

        _posts = await DailyNewsItemsService.GetDailyNewsItemsIncludeUserAndTagByTagNameAsync(TagName!,
            CurrentPage.Value - 1, PostItemsPerPage);

        await SetShowExportedFileAsync();
        AddTagPostsBreadCrumbs();
    }

    private async Task SetShowExportedFileAsync()
    {
        _tagId = _posts?.Data.FirstOrDefault()
            ?.Tags.FirstOrDefault(x => x.Name.Equals(TagName, StringComparison.OrdinalIgnoreCase))
            ?.Id;

        _isExportedPdfFileReady = _tagId.HasValue &&
                                  (await PdfExportService.GetExportFileLocationAsync(WhatsNewItemType.NewsTag,
                                      _tagId.Value))?.IsReady == true;
    }

    private void AddTagPostsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..NewsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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
