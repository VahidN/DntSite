using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemCommentBookmarkConfig : IEntityTypeConfiguration<DailyNewsItemCommentBookmark>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemCommentBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItemCommentBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
