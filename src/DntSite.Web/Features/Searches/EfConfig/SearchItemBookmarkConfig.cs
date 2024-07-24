using DntSite.Web.Features.Searches.Entities;

namespace DntSite.Web.Features.Searches.EfConfig;

public class SearchItemBookmarkConfig : IEntityTypeConfiguration<SearchItemBookmark>
{
    public void Configure(EntityTypeBuilder<SearchItemBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(entity => entity.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.SearchItemBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
