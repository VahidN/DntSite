using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.Models;

public class BlogPostModel
{
    public BlogPost? CurrentItem { set; get; }

    public BlogPost? NextItem { set; get; }

    public BlogPost? PreviousItem { set; get; }

    //public IList<Referrer> Referrers { set; get; }

    //public IList<BlogPost> InternalOutLinks { set; get; }
}
