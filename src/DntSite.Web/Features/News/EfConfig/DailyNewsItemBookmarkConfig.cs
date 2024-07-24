using DntSite.Web.Features.News.Entities;

namespace DntSite.Web.Features.News.EfConfig;

public class DailyNewsItemBookmarkConfig : IEntityTypeConfiguration<DailyNewsItemBookmark>
{
    public void Configure(EntityTypeBuilder<DailyNewsItemBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.DailyNewsItemBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
