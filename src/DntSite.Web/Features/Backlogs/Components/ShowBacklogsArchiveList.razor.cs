using DntSite.Web.Features.AppConfigs.Components;
using DntSite.Web.Features.AppConfigs.Services;
using DntSite.Web.Features.Backlogs.Entities;
using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Common.Utils.Pagings.Models;

namespace DntSite.Web.Features.Backlogs.Components;

public partial class ShowBacklogsArchiveList
{
    [Parameter] [EditorRequired] public required string MainTitle { set; get; }

    [Parameter] [EditorRequired] public PagedResultModel<Backlog>? Posts { set; get; }

    [Parameter] [EditorRequired] public int? CurrentPage { set; get; }

    [Parameter] [EditorRequired] public int ItemsPerPage { set; get; }

    [Parameter] [EditorRequired] public required string BasePath { set; get; }

    [CascadingParameter] internal ApplicationState ApplicationState { set; get; } = null!;

    private bool CanUserDeleteThisPost => ApplicationState.IsCurrentUserAdmin;

    private static List<string> GetTags(Backlog? post) => post?.Tags.Select(x => x.Name).ToList() ?? [];

    private bool CanUserEditThisPost(Backlog post)
        => ApplicationState.CanCurrentUserEditThisItem(post.UserId, post.Audit.CreatedAt);

    private static ManageBacklogModel GetBacklogStatModel(Backlog? post)
        => new()
        {
            ConvertedBlogPostId = post?.ConvertedBlogPostId,
            DaysEstimate = post?.DaysEstimate,
            TakenByUser = post?.DoneByUser,
            Id = post?.Id ?? 0,
            DoneDate = post?.DoneDate,
            StartDate = post?.StartDate
        };
}
