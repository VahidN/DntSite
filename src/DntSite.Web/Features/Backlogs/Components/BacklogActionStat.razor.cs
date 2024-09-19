using DntSite.Web.Features.Backlogs.Models;
using DntSite.Web.Features.Backlogs.RoutingConstants;

namespace DntSite.Web.Features.Backlogs.Components;

public partial class BacklogActionStat
{
    [MemberNotNullWhen(returnValue: true, nameof(Model))]
    private bool IsTaken => Model?.TakenByUser is not null;

    private string ConvertedBlogPostUrl => string.Create(CultureInfo.InvariantCulture,
        $"{BacklogsRoutingConstants.PostsBase}/{Model?.ConvertedBlogPostId}");

    [Parameter] [EditorRequired] public ManageBacklogModel? Model { set; get; }
}
