using DntSite.Web.Features.Posts.Entities;

namespace DntSite.Web.Features.Posts.EfConfig;

public class BlogPostCommentBookmarkConfig : IEntityTypeConfiguration<BlogPostCommentBookmark>
{
    public void Configure(EntityTypeBuilder<BlogPostCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BlogPostCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
