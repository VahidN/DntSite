using DntSite.Web.Features.Backlogs.Entities;

namespace DntSite.Web.Features.Backlogs.EfConfig;

public class BacklogCommentBookmarkConfig : IEntityTypeConfiguration<BacklogCommentBookmark>
{
    public void Configure(EntityTypeBuilder<BacklogCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(commentBookmark => commentBookmark.Parent)
            .WithMany(comment => comment.Bookmarks)
            .HasForeignKey(commentBookmark => commentBookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.BacklogCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
