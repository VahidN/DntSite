using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.Projects.RoutingConstants;

namespace DntSite.Web.Features.Projects.Components;

[Authorize]
public partial class ShowProjectFeedbacksBookmarks
{
    private const int ItemsPerPage = 10;

    [Parameter] public int? CurrentPage { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        AddBreadCrumbs();
    }

    private void AddBreadCrumbs()
        => ApplicationState.BreadCrumbs.AddRange([
            ..ProjectsBreadCrumbs.DefaultBreadCrumbs, ProjectsBreadCrumbs.ProjectIssuesBookmarksBreadCrumb
        ]);
}
