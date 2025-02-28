using DntSite.Web.Common.BlazorSsr.Utils;
using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.Posts.Services.Contracts;

namespace DntSite.Web.Features.Posts.Components;

public partial class ShowBlogPostsComments
{
    private const int PostItemsPerPage = 10;
    private const string MainPageTitle = "نظرات مطالب";
    private const string MainPageUrl = PostsRoutingConstants.PostsComments;

    private PagedResultModel<BlogPostComment>? _posts;

    [MemberNotNullWhen(returnValue: true, nameof(UserFriendlyName))]
    private bool HasUserFriendlyName => !string.IsNullOrWhiteSpace(UserFriendlyName);

    private string MainTitle => !HasUserFriendlyName ? MainPageTitle : MainUserPageTitle;

    private string MainUserPageTitle => string.Create(CultureInfo.InvariantCulture,
        $"آرشیو نظرات مطالب {UserFriendlyName}");

    private string MainUserPageUrl
        => !HasUserFriendlyName ? MainPageUrl : $"{MainPageUrl}/{Uri.EscapeDataString(UserFriendlyName)}";

    private string BasePath => !HasUserFriendlyName ? MainPageUrl : MainUserPageUrl;

    [Parameter] public string? UserFriendlyName { set; get; }

    [InjectComponentScoped] internal IBlogCommentsService BlogCommentsService { set; get; } = null!;

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

        _posts = await BlogCommentsService.GetLastPagedBlogCommentsAsNoTrackingAsync(CurrentPage.Value - 1,
            PostItemsPerPage);

        AddCommentsListBreadCrumbs();
    }

    private void AddCommentsListBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..PostsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
            {
                Title = MainTitle,
                Url = MainPageUrl,
                GlyphIcon = DntBootstrapIcons.BiTag
            }
        ]);

    private async Task ShowUserCommentsAsync()
    {
        CurrentPage ??= 1;

        _posts = await BlogCommentsService.GetLastPagedBlogCommentsAsNoTrackingAsync(UserFriendlyName!,
            CurrentPage.Value - 1, PostItemsPerPage);

        AddUserCommentsBreadCrumbs();
    }

    private void AddUserCommentsBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..PostsBreadCrumbs.DefaultBreadCrumbs, new BreadCrumb
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

    private static string GetPostAbsoluteUrl(BlogPostComment comment)
        => string.Create(CultureInfo.InvariantCulture, $"{PostsRoutingConstants.PostBase}/{comment.ParentId}");
}
