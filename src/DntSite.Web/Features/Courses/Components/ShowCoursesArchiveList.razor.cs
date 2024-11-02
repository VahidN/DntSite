using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Common.Utils.Pagings.Models;
using DntSite.Web.Features.Courses.Entities;

namespace DntSite.Web.Features.Courses.Components;

public partial class ShowCoursesArchiveList
{
    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    [Parameter] [EditorRequired] public PagedResultModel<Course>? Posts { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private static List<string> GetTags(Course? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private bool CanUserEditThisPost(Course post) => ApplicationState.CanCurrentUserEditThisItem(post.UserId);
}
