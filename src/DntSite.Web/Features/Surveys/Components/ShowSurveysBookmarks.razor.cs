using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Surveys.Entities;
using DntSite.Web.Features.Surveys.RoutingConstants;

namespace DntSite.Web.Features.Surveys.Components;

[Authorize]
public partial class ShowSurveysBookmarks
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

    private bool CanUserEditThisPost(Survey post)
        => ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt);

    private static List<string> GetTags(Survey? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private void AddBreadCrumbs() => ApplicationState.BreadCrumbs.AddRange([..SurveysBreadCrumbs.DefaultBreadCrumbs]);
}
