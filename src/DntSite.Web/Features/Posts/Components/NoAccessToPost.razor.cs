using DntSite.Web.Features.Posts.Entities;
using DntSite.Web.Features.Posts.RoutingConstants;

namespace DntSite.Web.Features.Posts.Components;

public partial class NoAccessToPost
{
    private string PostUrl
        => string.Create(CultureInfo.InvariantCulture, $"{PostsRoutingConstants.PostBase}/{BlogPost.Id}");

    [Parameter] [EditorRequired] public required BlogPost BlogPost { set; get; }
}
