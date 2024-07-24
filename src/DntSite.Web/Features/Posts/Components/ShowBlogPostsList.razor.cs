using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Components;

public partial class ShowBlogPostsList
{
    private string PageTitle => Invariant($"{MainTitle}، صفحه: {CurrentPage ?? 1}");

    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    [Parameter] [EditorRequired] public PagedResultModel<BlogPost>? BlogPosts { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.CurrentUser?.IsAdmin == true;

    private static List<string> GetTags(BlogPost? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private bool CanUserEditThisPost(BlogPost blogPost)
        => ApplicationState.CanCurrentUserEditThisItem(blogPost.UserId, blogPost.Audit.CreatedAt);
}
