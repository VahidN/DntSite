using DntSite.Web.Features.Persistence.BaseDomainEntities.Entities;

namespace DntSite.Web.Features.Posts.Entities;

public class BlogPostComment : BaseCommentsEntity<BlogPostComment, BlogPost, BlogPostCommentVisitor,
    BlogPostCommentBookmark, BlogPostCommentReaction>
{
    public bool EmailsSent { set; get; }
}
