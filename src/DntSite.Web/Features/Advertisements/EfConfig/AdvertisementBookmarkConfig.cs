using DntSite.Web.Features.Advertisements.Entities;

namespace DntSite.Web.Features.Advertisements.EfConfig;

public class AdvertisementBookmarkConfig : IEntityTypeConfiguration<AdvertisementBookmark>
{
    public void Configure(EntityTypeBuilder<AdvertisementBookmark> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.HasOne(bookmark => bookmark.Parent)
            .WithMany(advertisement => advertisement.Bookmarks)
            .HasForeignKey(bookmark => bookmark.ParentId)
            .IsRequired();

        builder.HasOne(entity => entity.User)
            .WithMany(user => user.AdvertisementBookmarks)
            .HasForeignKey(entity => entity.UserId)
            .IsRequired(false);
    }
}
