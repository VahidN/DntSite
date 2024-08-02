using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;
using DntSite.Web.Features.UserProfiles.Models;

namespace DntSite.Web.Features.Posts.Components;

public partial class SimpleDraftsList
{
    [Parameter] [EditorRequired] public IList<BlogPostDraft>? BlogPostDrafts { set; get; }

    [Parameter] [EditorRequired] public required string Title { set; get; }

    [Parameter] [EditorRequired] public required string TitleUrl { set; get; }

    [Parameter] [EditorRequired] public CurrentUserModel? CurrentUser { set; get; }

    [Parameter] [EditorRequired] public bool ShowBriefDescription { set; get; }

    [Inject] public IProtectionProviderService ProtectionProvider { set; get; } = null!;

    private static string GetShowDraftLink(int itemId) => Invariant($"{PostsRoutingConstants.ShowDraftBase}/{itemId}");

    private static string GetTagUrl(string tagName) => $"{PostsRoutingConstants.Tag}/{Uri.EscapeDataString(tagName)}";
}
