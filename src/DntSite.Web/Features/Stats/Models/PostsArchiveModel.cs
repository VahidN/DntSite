using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Stats.Models;

public class PostsArchiveModel
{
    public PostsArchiveFavUserRatings? PostsArchiveFavUserRatings { set; get; }

    public IList<BlogPost>? BlogPosts { set; get; }
}
