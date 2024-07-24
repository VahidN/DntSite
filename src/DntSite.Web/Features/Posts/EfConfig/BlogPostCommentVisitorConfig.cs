using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostCommentVisitorConfig : IEntityTypeConfiguration<BlogPostCommentVisitor>
{
    public void Configure(EntityTypeBuilder<BlogPostCommentVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostCommentVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
