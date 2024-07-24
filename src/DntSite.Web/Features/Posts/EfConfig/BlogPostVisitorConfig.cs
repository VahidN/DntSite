using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostVisitorConfig : IEntityTypeConfiguration<BlogPostVisitor>
{
    public void Configure(EntityTypeBuilder<BlogPostVisitor> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(visitor => visitor.Parent)
            .WithMany(comment => comment.Visitors)
            .HasForeignKey(visitor => visitor.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostVisitors)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
