using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Projects.Entities;
using DntSite.Web.Features.Projects.RoutingConstants;

namespace DntSite.Web.Features.Projects.Components;

[Authorize]
public partial class ShowProjectsBookmarks
{
    private const int ItemsPerPage = 10;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        AddBreadCrumbs();
    }

    private bool CanUserEditThisPost(Project post)
        => ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt);

    private static List<string> GetTags(Project? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..ProjectsBreadCrumbs.DefaultBreadCrumbs, ProjectsBreadCrumbs.ProjectsBookmarksBreadCrumb
        ]);
}
