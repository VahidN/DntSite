using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Services.Contracts;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.Components;

public partial class Tag
{
    private const int PostItemsPerPage = 10;
    private const int TagItemsPerPage = 30;
    private const string MainPageTitle = "گروه‌های مطالب";
    private const string MainPageUrl = PostsRoutingConstants.Tag;

    private PagedResultModel<BlogPost>? _blogPosts;

    private int? _tagId;
    private IList<(string Name, int Id, int InUseCount)>? _tags;
    private int _totalTagItemsCount;

    [MemberNotNullWhen(returnValue: true, nameof(TagName))]
    private bool HasTag => !string.IsNullOrWhiteSpace(TagName);

    private string MainTitle => !HasTag ? MainPageTitle : MainTagPageTitle;

    private string MainTagPageTitle => string.Create(CultureInfo.InvariantCulture, $"آرشیو گروه‌های مطالب {TagName}");

    private string MainTagPageUrl => !HasTag ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(TagName)}";

    private string BasePath => !HasTag ? MainPageUrl : MainTagPageUrl;

    [Parameter] public string? TagName { set; get; }

    [InjectComponentScoped] internal IBlogPostsService BlogPostsService { set; get; } = null!;

    [InjectComponentScoped] internal ITagsService TagsService { set; get; } = null!;

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

        var results = await TagsService.GetPagedAllPostTagsListAsNoTrackingAsync(CurrentPage.Value - 1,
            TagItemsPerPage);

        _tags = results.Data.Select(x => (x.Name, x.Id, x.InUseCount)).ToList();
        _totalTagItemsCount = results.TotalItems;

        AddTagsListBreadCrumbs();
    }

    private void AddTagsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..PostsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiTag
            }
        ]);

    private async Task ShowTagPostsAsync()
    {
        CurrentPage ??= 1;

        _blogPosts =
            await BlogPostsService.GetLastBlogPostsByTagIncludeAuthorAsync(TagName!, CurrentPage.Value - 1,
                PostItemsPerPage);

        _tagId = _blogPosts.Data?.FirstOrDefault()
            ?.Tags.FirstOrDefault(x => x.Name.Equals(TagName, StringComparison.OrdinalIgnoreCase))
            ?.Id;

        AddTagPostsBreadCrumbs();
    }

    private void AddTagPostsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..PostsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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
